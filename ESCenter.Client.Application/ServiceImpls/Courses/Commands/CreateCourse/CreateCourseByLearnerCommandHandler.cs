using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.CreateCourse;

public class CreateCourseByLearnerCommandHandler(
    ICourseRepository courseRepository,
    ICustomerRepository customerRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseByLearnerCommandHandler> logger)
    : CommandHandlerBase<CreateCourseByLearnerCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseByLearnerCommand byLearnerCommand,
        CancellationToken cancellationToken)
    {
        var course = Course.Create(
            byLearnerCommand.CourseCreateForLearnerDto.Title,
            byLearnerCommand.CourseCreateForLearnerDto.Description,
            byLearnerCommand.CourseCreateForLearnerDto.LearningMode,
            byLearnerCommand.CourseCreateForLearnerDto.Fee,
            byLearnerCommand.CourseCreateForLearnerDto.Fee * 2,
            "Dollar",
            byLearnerCommand.CourseCreateForLearnerDto.GenderRequirement,
            byLearnerCommand.CourseCreateForLearnerDto.AcademicLevelRequirement,
            byLearnerCommand.CourseCreateForLearnerDto.LearnerGender,
            byLearnerCommand.CourseCreateForLearnerDto.LearnerName,
            byLearnerCommand.CourseCreateForLearnerDto.NumberOfLearner,
            byLearnerCommand.CourseCreateForLearnerDto.ContactNumber,
            byLearnerCommand.CourseCreateForLearnerDto.MinutePerSession,
            null,
            byLearnerCommand.CourseCreateForLearnerDto.SessionPerWeek,
            byLearnerCommand.CourseCreateForLearnerDto.Address,
            SubjectId.Create(byLearnerCommand.CourseCreateForLearnerDto.SubjectId),
            null);

        if (currentUserService.IsAuthenticated)
        {
            var learner = await customerRepository.GetAsync(
                CustomerId.Create(currentUserService.UserId),
                cancellationToken);

            if (learner is not null)
            {
                course.SetLearnerId(learner.Id);
            }
        }

        //Handle publish event to notification service
        await courseRepository.InsertAsync(course, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}