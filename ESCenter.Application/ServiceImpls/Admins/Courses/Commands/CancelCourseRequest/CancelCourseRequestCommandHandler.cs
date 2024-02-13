using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.CourseRequests.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Commands.CancelCourseRequest;

public class CancelCourseRequestCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<CancelCourseRequestCommandHandler> logger)
    : CommandHandlerBase<CancelCourseRequestCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CancelCourseRequestCommand command, CancellationToken cancellationToken)
    {
        var course =
            await courseRepository.GetCourseByCourseRequestId(CourseRequestId.Create(command.CourseRequestId),
                cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        // TODO: This will be domain service
        var courseRequest =
            course.CourseRequests.FirstOrDefault(x => x.Id == CourseRequestId.Create(command.CourseRequestId));

        if (courseRequest is null)
        {
            return Result.Fail(CourseAppServiceErrors.NonExistCourseRequestError);
        }

        courseRequest.Cancel();

        // Check does course state still go correctly  

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(CourseRequestAppServiceErrors.FailToCancelCourseRequestError);
        }

        return Result.Success();
    }
}