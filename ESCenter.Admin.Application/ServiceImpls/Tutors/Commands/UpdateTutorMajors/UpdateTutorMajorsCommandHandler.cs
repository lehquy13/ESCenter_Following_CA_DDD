using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorMajors;

public class UpdateTutorMajorsCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository)
    : CommandHandlerBase<UpdateTutorMajorsCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateTutorMajorsCommand command,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(TutorId.Create(command.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var subjectIds = command.MajorIds.Select(SubjectId.Create).ToList();
        var subjects = await subjectRepository.GetListByIdsAsync(subjectIds, cancellationToken);

        var tutorMajors = subjects
            .Select(x => TutorMajor.Create(tutor.Id, x.Id, x.Name))
            .ToList();

        tutor.UpdateAllMajor(tutorMajors);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserError.FailToUpdateChangeVerificationRequest);
        }

        return Result.Success();
    }
}