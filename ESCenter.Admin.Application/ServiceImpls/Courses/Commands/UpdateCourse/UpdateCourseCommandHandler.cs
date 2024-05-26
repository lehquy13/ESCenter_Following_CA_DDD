using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateCourseCommandHandler> logger)
    : CommandHandlerBase<UpdateCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        var course = await courseRepository
            .GetAsync(CourseId.Create(command.CourseUpdateDto.Id), cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var updateStatus = command.CourseUpdateDto.Status.ToEnum<Status>();

        course.UpdateCourse(
            command.CourseUpdateDto.Title,
            command.CourseUpdateDto.Description,
            command.CourseUpdateDto.LearningMode.ToEnum<LearningMode>(),
            command.CourseUpdateDto.SectionFee,
            command.CourseUpdateDto.ChargeFee,
            command.CourseUpdateDto.GenderRequirement.ToEnum<Gender>(),
            command.CourseUpdateDto.AcademicLevelRequirement.ToEnum<AcademicLevel>(),
            command.CourseUpdateDto.LearnerGender.ToEnum<Gender>(),
            command.CourseUpdateDto.LearnerName,
            command.CourseUpdateDto.NumberOfLearner,
            command.CourseUpdateDto.ContactNumber,
            command.CourseUpdateDto.SessionDuration,
            command.CourseUpdateDto.SessionPerWeek,
            command.CourseUpdateDto.Address,
            updateStatus,
            SubjectId.Create(command.CourseUpdateDto.SubjectId));

        if (command.CourseUpdateDto.TutorId != Guid.Empty &&
            command.CourseUpdateDto.TutorId != course.TutorId?.Value)
        {
            var tutorId = TutorId.Create(command.CourseUpdateDto.TutorId);

            if (updateStatus != Status.Confirmed && updateStatus != Status.OnProgressing)
            {
                return Result.Fail(CourseAppServiceErrors.InvalidStatusForAssignTutor);
            }

            course.AssignTutor(tutorId);
        }
        else if (command.CourseUpdateDto.TutorId == Guid.Empty && course.TutorId is not null)
        {
            course.UnAssignTutor();
        }

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail(CourseAppServiceErrors.UpdateCourseFailedWhileSavingChanges)
            : Result.Success();
    }
}