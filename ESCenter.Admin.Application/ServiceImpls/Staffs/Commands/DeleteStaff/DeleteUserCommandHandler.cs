using ESCenter.Domain;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.DeleteStaff;

public class DeleteStaffCommandHandler(
    IIdentityService identityRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteStaffCommandHandler> logger,
    IPublisher publisher
) : CommandHandlerBase<DeleteStaffCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var result = await identityRepository.RemoveAsync(CustomerId.Create(request.Id));

        if (!result.IsSuccess && await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(StaffAppServiceError.FailToDeleteStaffErrorWhileSavingChanges);
        }

        var message = "Learner " + request.Id + " at " +
                      DateTime.UtcNow.ToLongDateString();

        await publisher.Publish(
            new NewDomainObjectCreatedEvent(
                request.Id.ToString(),
                message,
                NotificationEnum.Learner),
            cancellationToken);

        return Result.Success();
    }
}