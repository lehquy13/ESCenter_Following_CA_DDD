using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Commands.CreateCourse;

public class CreateCourseCommandHandler(
    IMapper mapper,
    IPublisher publisher,
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseCommandHandler> logger)
    : CommandHandlerBase<CreateCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var course = mapper.Map<Course>(command.CourseForCreateDto);

            //TODO: Update when user repository is added
            // if (!string.IsNullOrWhiteSpace(courseDto.ContactNumber))
            // {
            //     //Class was created by a system user
            //     var user = await _userRepository.GetUserByEmail(command.Email);
            //     if (user != null)
            //     {
            //         course.LearnerId = user.Id;
            //     }
            // }

            //Handle publish event to notification service
            await courseRepository.InsertAsync(course, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            var message = "New class: " + course.Title + " at " + course.CreationTime.ToLongDateString();
            await publisher.Publish(new NewObjectCreatedEvent(course.Id.Value.ToString(), message, NotificationEnum.Course), cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when class is adding or updating." + ex.Message);
        }
    }
}