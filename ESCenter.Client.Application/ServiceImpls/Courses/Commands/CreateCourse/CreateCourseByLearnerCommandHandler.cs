using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.CreateCourse;

public class CreateCourseByLearnerCommandHandler(
    IMapper mapper,
    IPublisher publisher,
    ICourseRepository courseRepository,
    ICustomerRepository customerRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseByLearnerCommandHandler> logger)
    : CommandHandlerBase<CreateCourseByLearnerCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseByLearnerCommand byLearnerCommand,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = mapper.Map<Course>(byLearnerCommand.CourseCreateForLearnerDto);

            if (currentUserService.UserId == Guid.Empty)
            {
                var learner = await customerRepository.GetAsync(
                    CustomerId.Create(currentUserService.UserId),
                    cancellationToken);

                if (learner is not null)
                {
                    course.SetLearnerId(learner.Id);
                }
            }

            //Handle publish event to notification service
            await courseRepository.InsertAsync(course, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);


            var message = "New class: " + course.Title + " was created by " + course.LearnerId +
                          " at " + course.CreationTime.ToLongDateString();
            await publisher.Publish(
                new NewObjectCreatedEvent(course.Id.Value.ToString(), message, NotificationEnum.Course),
                cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when class is adding or updating." + ex.Message);
        }
    }
}