using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;

public class CreateCourseRequestCommandHandler(
    ICourseRepository courseRepository,
    IPublisher publisher,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseRequestCommandHandler> logger)
    : CommandHandlerBase<CreateCourseRequestCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseRequestCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseRepository.GetAsync(
                CourseId.Create(command.CourseRequestForCreateDto.CourseId), cancellationToken);

            if (course is null)
            {
                return Result.Fail(CourseRequestAppServiceErrors.NonExistCourseError);
            }

            var courseRequestToCreate = CourseRequest.Create(
                TutorId.Create(command.CourseRequestForCreateDto.TutorId),
                CourseId.Create(command.CourseRequestForCreateDto.CourseId),
                string.Empty
            );

            course.Request(courseRequestToCreate);

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) < 0)
            {
                return Result.Fail(CourseRequestAppServiceErrors.FailToCreateCourseRequestError);
            }

            var message = $"New request class with Id {courseRequestToCreate.Id.Value} " +
                          $"at {courseRequestToCreate.CreationTime.ToLongDateString()}";
            await publisher.Publish(new NewObjectCreatedEvent(courseRequestToCreate.CourseId.Value.ToString(), message,
                NotificationEnum.Course), cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            Logger.LogError("Fail to create course request with {Message}", e.InnerException!.Message);
            return Result.Fail(CourseRequestAppServiceErrors.FailToCreateCourseRequestError);
        }
    }
}