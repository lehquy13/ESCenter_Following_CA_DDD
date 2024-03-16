using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public class ReviewCourseCommandHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IPublisher publisher,
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
            return Result.Fail(CourseError.NonExistCourseError);
        }

        if (courseFromDb.Status != Status.Confirmed || courseFromDb.TutorId is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseNotConfirmedError);
        }

        if (courseFromDb.LearnerId != IdentityGuid.Create(currentUserService.UserId))
        {
            return Result.Fail(CourseAppServiceErrors.IncorrectUserOfCourseError);
        }

        courseFromDb.ReviewCourse(query.Rate, query.Detail);

        var tutorToUpdateRateQuery =
            from tutorQ in tutorRepository.GetAll()
            join courseQ in courseRepository.GetAll() on tutorQ.Id equals courseQ.TutorId into courseQs
            where tutorQ.Id == courseFromDb.TutorId
            select new
            {
                Tutor = tutorQ,
                ReviewRate = courseQs
                    .Where(x => x.Review != null)
                    .Select(x => x.Review!.Rate),
            };

        var tutorToUpdate = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorToUpdateRateQuery, true,
            cancellationToken);

        if (tutorToUpdate is null)
        {
            Logger.LogError("Doesnt have any tutor to update rate");
            return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
        }

        tutorToUpdate.Tutor.UpdateRate(tutorToUpdate.ReviewRate.Average(x => x));

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(CourseAppServiceErrors.FailToReviewCourse);
        }

        var message = "Review class: " + courseFromDb.Title + " at " + courseFromDb.CreationTime.ToLongDateString();
        await publisher.Publish(new NewObjectCreatedEvent(courseFromDb.Id.Value.ToString(), message,
            NotificationEnum.Course), cancellationToken);

        return Result.Success();
    }
}