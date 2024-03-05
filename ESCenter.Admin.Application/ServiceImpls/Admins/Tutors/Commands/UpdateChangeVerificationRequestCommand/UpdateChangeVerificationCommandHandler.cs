using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Tutors.Commands.UpdateChangeVerificationRequestCommand;

public class UpdateChangeVerificationCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorRepository tutorRepository)
    : CommandHandlerBase<UpdateChangeVerificationCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateChangeVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(TutorId.Create(command.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var result = tutor.ModifyChangeVerificationRequest(command.IsApproved);
        
        if (result.IsFailure)
        {
            return Result.Fail(result.Error);
        }
        
        tutor.Verify(true);

        // TODO: Check does old verification info need to be deleted?
        //await tutorRepository.RemoveChangeVerification(tutor.ChangeVerificationRequest!.Id);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToUpdateChangeVerificationRequest);
        }

        return Result.Success();
    }
}