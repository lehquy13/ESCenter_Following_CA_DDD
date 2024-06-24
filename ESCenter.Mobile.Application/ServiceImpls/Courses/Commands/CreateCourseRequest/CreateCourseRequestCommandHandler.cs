using ESCenter.Domain;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;

public class CreateCourseRequestCommandHandler(
    ICourseDomainService courseDomainService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseRequestCommandHandler> logger)
    : CommandHandlerBase<CreateCourseRequestCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await courseDomainService.RequestCourse(
            CourseId.Create(command.CourseRequestForCreateDto.CourseId),
            CustomerId.Create(currentUserService.UserId));
        
        if(result.IsFailure )
        {
            return result.Error;
        }

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) < 0)
        {
            return Result.Fail(CourseRequestAppServiceErrors.FailToCreateCourseRequestError);
        }

        return Result.Success();
    }
}