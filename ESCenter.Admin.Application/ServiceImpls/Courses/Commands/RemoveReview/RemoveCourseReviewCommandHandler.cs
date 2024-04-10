using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.RemoveReview;

public class RemoveCourseReviewCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<RemoveCourseReviewCommandHandler> logger)
    : CommandHandlerBase<RemoveCourseReviewCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RemoveCourseReviewCommand command, CancellationToken cancellationToken)
    {
        var courseFromDb = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (courseFromDb is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var result = courseFromDb.RemoveReview();

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail("Fail to remove the review");
        }

        return Result.Success();
    }
}