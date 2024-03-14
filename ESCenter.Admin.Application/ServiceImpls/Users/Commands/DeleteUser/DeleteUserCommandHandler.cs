using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Admin.Application.ServiceImpls.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IIdentityRepository identityRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteUserCommandHandler> logger,
    IPublisher publisher
) : CommandHandlerBase<DeleteUserCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await identityRepository.RemoveAsync(IdentityGuid.Create(request.Id), cancellationToken);

        var message = "Learner " + request.Id + " at " +
                      DateTime.Now.ToLongDateString();

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserAppServiceError.FailToDeleteUserErrorWhileSavingChanges);
        }

        await publisher.Publish(new NewObjectCreatedEvent(request.Id.ToString(), message, NotificationEnum.Learner),
            cancellationToken);

        return Result.Success();
    }
}