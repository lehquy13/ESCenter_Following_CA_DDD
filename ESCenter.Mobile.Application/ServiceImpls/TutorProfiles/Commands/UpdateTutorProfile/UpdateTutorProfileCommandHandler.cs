using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Specifications.Subjects;
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
        try
        {
            var tutorId = TutorId.Create(currentUserService.UserId);
            var tutor = await tutorRepository.GetAsync(tutorId, cancellationToken);

            // Check if the tutor exist
            if (tutor is null)
            {
                return Result.Fail(UserError.NonExistTutorError);
            }

            // Collect major ids
            var subjectListAboutToUpdate =
                await subjectRepository.GetListAsync(
                    new SubjectListByNameSpec(command.TutorBasicUpdateDto.Majors),
                    cancellationToken);

            foreach (var major in tutor.TutorMajors)
            {
                if (subjectListAboutToUpdate.All(update => update.Id != major.SubjectId))
                {
                    tutor.RemoveMajor(major);
                }
            }

            // Now current major only contains the subjects that are in the subjectListAboutToUpdate
            // Then we add new majors to the tutor
            var addMajors = subjectListAboutToUpdate
                .Where(x => tutor.TutorMajors.All(update => update.SubjectId != x.Id))
                .Select(x => TutorMajor.Create(tutorId, x.Id, x.Name))
                .ToList();

            foreach (var major in addMajors)
            {
                tutor.AddTutorMajor(major);
            }

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(TutorAppServiceError.FailToUpdateTutorWhileSavingChanges);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when tutor is adding or updating: " + ex.Message);
        }
    }
}