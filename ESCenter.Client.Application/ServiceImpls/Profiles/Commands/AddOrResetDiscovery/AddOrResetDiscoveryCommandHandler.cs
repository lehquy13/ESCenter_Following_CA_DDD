using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.DiscoveryUsers.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;

public class AddOrResetDiscoveryCommandHandler(
    IUnitOfWork unitOfWork,
    ICustomerRepository customerRepository,
    IAppLogger<AddOrResetDiscoveryCommandHandler> logger,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IRepository<DiscoveryUser, DiscoveryUserId> discoveryUserRepository)
    : CommandHandlerBase<AddOrResetDiscoveryCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(AddOrResetDiscoveryCommand request, CancellationToken cancellationToken)
    {
        var userId = CustomerId.Create(request.UserId);
        var user = await customerRepository.GetAsync(CustomerId.Create(request.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Fail(UserProfileAppServiceError.NonExistUserError);
        }

        var discoveryQuery =
            from discoveryUser in discoveryUserRepository.GetAll()
            where discoveryUser.UserId == userId || request.DiscoveryIds.Contains(discoveryUser.DiscoveryId.Value)
            select discoveryUser;

        var userDiscovery =
            await asyncQueryableExecutor.ToListAsync(discoveryQuery, true, cancellationToken);

        // select the discoveries that are already in the user's discovery list
        var discoveriesInUserDiscoveryList = userDiscovery
            .Where(discovery => request.DiscoveryIds.Contains(discovery.DiscoveryId.Value))
            .ToList();

        // update to db
        discoveriesInUserDiscoveryList =
            discoveriesInUserDiscoveryList
                .Select(discovery => discovery)
                .ToList();

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserProfileAppServiceError.FailToAddOrResetDiscoveryErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}