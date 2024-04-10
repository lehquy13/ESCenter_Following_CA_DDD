using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public class ReviewCourseCommandHandler(
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ReviewCourseCommandHandler> logger)
    : CommandHandlerBase<ReviewCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ReviewCourseCommand query, CancellationToken cancellationToken)
    {
        var courseFromDb = await courseRepository.GetAsync(CourseId.Create(query.CourseId),
            cancellationToken);

        if (courseFromDb == null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        if (courseFromDb.Status != Status.Confirmed || courseFromDb.TutorId is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseNotConfirmedError);
        }

        if (courseFromDb.LearnerId != CustomerId.Create(currentUserService.UserId))
        {
            return Result.Fail(CourseAppServiceErrors.IncorrectUserOfCourseError);
        }

        courseFromDb.ReviewCourse(query.Rate, query.Detail);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(CourseAppServiceErrors.FailToReviewCourse);
        }

        return Result.Success();
    }
}