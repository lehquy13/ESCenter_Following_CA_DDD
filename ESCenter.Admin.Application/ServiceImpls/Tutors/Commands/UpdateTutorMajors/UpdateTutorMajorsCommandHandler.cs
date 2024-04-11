using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorMajors;

public class UpdateTutorMajorsCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorDomainService tutorDomainService)
    : CommandHandlerBase<UpdateTutorMajorsCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateTutorMajorsCommand command,
        CancellationToken cancellationToken)
    {
        var result = await tutorDomainService.UpdateTutorMajorsAsync(TutorId.Create(command.TutorId), command.MajorIds);

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToUpdateChangeVerificationRequest);
        }

        return Result.Success();
    }
}