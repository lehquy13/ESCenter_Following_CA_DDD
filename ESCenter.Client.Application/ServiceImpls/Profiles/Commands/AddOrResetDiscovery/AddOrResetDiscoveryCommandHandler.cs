using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.DiscoveryUsers.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;

public class AddOrResetDiscoveryCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IRepository<Discovery, DiscoveryId> discoveryRepository,
    IAppLogger<AddOrResetDiscoveryCommandHandler> logger,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IRepository<DiscoveryUser, DiscoveryUserId> discoveryUserRepository)
    : CommandHandlerBase<AddOrResetDiscoveryCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(AddOrResetDiscoveryCommand request, CancellationToken cancellationToken)
    {
        var userId = CustomerId.Create(currentUserService.UserId);

        var discoveryQuery = discoveryUserRepository.GetAll()
            .Join(discoveryRepository.GetAll(),
                discoveryUser => discoveryUser.DiscoveryId,
                discovery => discovery.Id,
                (discoveryUser, discovery) => new { discoveryUser, discovery });

        var userDiscovery =
            await asyncQueryableExecutor.ToListAsync(discoveryQuery, true, cancellationToken);

        var discoverIds = userDiscovery.Select(x => x.discovery.Id.Value);

        // select the discoveries that are not in the user's discovery list
        var discoveriesNotInUserDiscoveryList = request.DiscoveryIds
            .Where(discoveryId => !discoverIds.Contains(discoveryId))
            .Select(discoveryId => DiscoveryUser.Create(DiscoveryId.Create(discoveryId), userId))
            .ToList();

        await discoveryUserRepository.InsertManyAsync(discoveriesNotInUserDiscoveryList, cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserProfileAppServiceError.FailToAddOrResetDiscoveryErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}