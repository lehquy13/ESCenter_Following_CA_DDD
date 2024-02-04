using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.UpdateChangeVerificationRequestCommand;

public class UpdateChangeVerificationCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorRepository tutorRepository)
    : CommandHandlerBase<UpdateChangeVerificationCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateChangeVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(IdentityGuid.Create(command.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var result = tutor.ChangeVerificationRequests.FirstOrDefault(x => x.Id == command.RequestId);

        if (result is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistChangeVerificationRequest);
        }

        if (command.IsApproved)
        {
            result.Approve();
        }
        else
        {
            result.Reject();
        }

        tutor.Verify(true);

        var newTutorVerificationInfos = result.ChangeVerificationRequestDetails.Select(x =>
            TutorVerificationInfo.Create(x.ImageUrl, tutor.Id)).ToList();

        tutor.UpdateTutorVerificationInfo(newTutorVerificationInfos);

        // TODO: Check does old verification info need to be deleted?
        //changeVerificationRequestRepository.Delete(result);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToUpdateChangeVerificationRequest);
        }

        return Result.Success();
    }
}