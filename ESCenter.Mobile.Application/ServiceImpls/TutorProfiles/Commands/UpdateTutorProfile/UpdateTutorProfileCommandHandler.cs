using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Mobile.Application.ServiceImpls.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;

public class UpdateTutorInformationCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateTutorInformationCommandHandler> logger,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    ISubjectRepository subjectRepository)
    : CommandHandlerBase<UpdateTutorInformationCommand>(unitOfWork,
        logger)
{
    public override async Task<Result> Handle(UpdateTutorInformationCommand command,
        CancellationToken cancellationToken)
    {
        var tutorId = CustomerId.Create(currentUserService.UserId);
        var tutor = await tutorRepository.GetTutorByUserId(tutorId, cancellationToken);

        // Check if the tutor exist
        if (tutor is null)
        {
            return Result.Fail(UserError.NonExistTutorError);
        }

        tutor.UpdateBasicInformation(command.TutorBasicUpdateDto.University,
            command.TutorBasicUpdateDto.AcademicLevel.ToEnum<AcademicLevel>());

        // Collect major ids
        var subjectListAboutToUpdate =
            await subjectRepository.GetListByIdsAsync(command.TutorBasicUpdateDto.Majors.Select(SubjectId.Create),
                cancellationToken);

        var majorsToRemove = new List<TutorMajor>();

        foreach (var major in tutor.TutorMajors)
        {
            if (subjectListAboutToUpdate.All(update => update.Id != major.SubjectId))
            {
                majorsToRemove.Add(major);
            }
        }

        foreach (var major in majorsToRemove)
        {
            tutor.RemoveMajor(major);
        }

        // Now current major only contains the subjects that are in the subjectListAboutToUpdate
        // Then we add new majors to the tutor
        var addMajors = subjectListAboutToUpdate
            .Where(x => tutor.TutorMajors.All(update => update.SubjectId != x.Id))
            .Select(x => TutorMajor.Create(tutor.Id, x.Id, x.Name))
            .ToList();

        foreach (var major in addMajors)
        {
            tutor.AddTutorMajor(major);
        }

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail(TutorAppServiceError.FailToUpdateTutorWhileSavingChanges)
            : Result.Success();
    }
}