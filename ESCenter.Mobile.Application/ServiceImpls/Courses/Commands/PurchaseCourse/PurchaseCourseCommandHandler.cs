using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices;
using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.PurchaseCourse;

public class PurchaseCourseCommandHandler(
    ICourseDomainService courseDomainService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<PurchaseCourseCommandHandler> logger)
    : CommandHandlerBase<PurchaseCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(PurchaseCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await courseDomainService.PurchaseCourse(CourseId.Create(command.CourseId),
            CustomerId.Create(currentUserService.UserId));

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(result.Error);
        }

        return Result.Success();
    }
}