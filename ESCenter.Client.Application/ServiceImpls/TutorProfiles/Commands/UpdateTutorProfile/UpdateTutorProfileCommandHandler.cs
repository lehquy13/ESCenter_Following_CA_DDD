using ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubject;
using ESCenter.Client.Application.ServiceImpls.Tutors;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Specifications.Subjects;
using ESCenter.Domain.Specifications.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;

public class UpdateTutorInformationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IAppLogger<UpdateTutorInformationCommandHandler> logger,
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository)
    : CommandHandlerBase<UpdateTutorInformationCommand>(unitOfWork,
        logger)
{
    public override async Task<Result> Handle(UpdateTutorInformationCommand command,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(new TutorByCustomerIdSpec(CustomerId.Create(currentUserService.UserId)), cancellationToken);

        // Check if the tutor exist
        if (tutor is null)
        {
            return Result.Fail(UserError.NonExistTutorError);
        }
        
        tutor.UpdateBasicInformation(command.TutorBasicUpdateDto.University, command.TutorBasicUpdateDto.AcademicLevel);

        // Collect major ids
        var subjectListAboutToUpdate =
            await subjectRepository.GetListAsync(
                new SubjectListByIdsSpec(command.TutorBasicUpdateDto.MajorIds),
                cancellationToken);

        var majorsToRemove = tutor.TutorMajors
            .Where(major => subjectListAboutToUpdate
                .All(update =>
                    update.Id != major.SubjectId)
            ).ToList();

        foreach (var major in majorsToRemove)
        {
            tutor.RemoveMajor(major);
        }

        var addMajors = subjectListAboutToUpdate
            .Where(x => tutor.TutorMajors.All(update => update.SubjectId != x.Id))
            .Select(x => TutorMajor.Create(tutor.Id, x.Id, x.Name))
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
}