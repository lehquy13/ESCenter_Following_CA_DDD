using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateCourseCommandHandler> logger)
    : CommandHandlerBase<UpdateCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseRepository
                .GetAsync(CourseId.Create(command.CourseUpdateDto.Id), cancellationToken);

            if (course is null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

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
                command.CourseUpdateDto.Status.ToEnum<Status>(),
                SubjectId.Create(command.CourseUpdateDto.SubjectId));

            if (command.CourseUpdateDto.TutorId != Guid.Empty && command.CourseUpdateDto.TutorId != course.TutorId?.Value)
            {
                var tutorId = TutorId.Create(command.CourseUpdateDto.TutorId);
                
                course.AssignTutor(tutorId);
                
                foreach (var courseRequest in course.CourseRequests)
                {
                    if (courseRequest.TutorId == tutorId)
                    {
                        courseRequest.Approved();
                    }
                    else
                    {
                        courseRequest.Cancel();
                    }
                }
            }
            else if (command.CourseUpdateDto.TutorId == Guid.Empty && course.TutorId is not null)
            {
                course.UnAssignTutor();
            }

            return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0 ? Result.Fail(CourseAppServiceErrors.UpdateCourseFailedWhileSavingChanges) : Result.Success();

            // Clear cache
            // var defaultRequest = new GetAllClassInformationsQuery();
            // _cache.Remove(defaultRequest.GetType() + JsonConvert.SerializeObject(defaultRequest));
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when class is updating." + ex.Message);
        }
    }
}