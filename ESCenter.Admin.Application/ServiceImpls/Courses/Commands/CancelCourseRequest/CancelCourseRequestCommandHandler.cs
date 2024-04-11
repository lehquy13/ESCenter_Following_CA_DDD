using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.DomainServices;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.CancelCourseRequest;

public class CancelCourseRequestCommandHandler(
    ICourseDomainService courseDomainService,
    IUnitOfWork unitOfWork,
    IAppLogger<CancelCourseRequestCommandHandler> logger)
    : CommandHandlerBase<CancelCourseRequestCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CancelCourseRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await courseDomainService.CancelCourseRequest(
            CourseId.Create(command.CourseId),
            CourseRequestId.Create(command.CourseRequestId),
            command.Description);

        // Check does course state still go correctly  

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(CourseRequestAppServiceErrors.FailToCancelCourseRequestError);
        }

        return Result.Success();
    }
}