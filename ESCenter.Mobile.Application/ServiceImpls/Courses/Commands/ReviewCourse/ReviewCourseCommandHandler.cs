using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public class ReviewCourseCommandHandler(
    ICourseDomainService courseDomainService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ReviewCourseCommandHandler> logger)
    : CommandHandlerBase<ReviewCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ReviewCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await courseDomainService.ReviewCourse(
            CourseId.Create(command.CourseId),
            command.Rate,
            command.Detail,
            CustomerId.Create(currentUserService.UserId));
        if (result.IsFailure)
        {
            return result;
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}