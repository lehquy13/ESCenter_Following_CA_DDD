using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;
public record CancelCourseWithRefundCommand(
    Guid CourseId,
    string Note = "Course has been canceled due to some reason") : ICommandRequest;

public class CancelCourseWithRefundCommandCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<CancelCourseRequestCommandHandler> logger)
    : CommandHandlerBase<CancelCourseWithRefundCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CancelCourseWithRefundCommand command,
        CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (course == null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var result = course.RefundCourse(command.Note);

        // Check does course state still go correctly  

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(CourseRequestAppServiceErrors.FailToCancelCourseRequestError);
        }

        return Result.Success();
    }
}