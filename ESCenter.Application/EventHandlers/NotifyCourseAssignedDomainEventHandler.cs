using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers;

public class NotifyCourseAssignedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IAppLogger<NotifyCourseAssignedDomainEventHandler> logger
) : INotificationHandler<TutorAssignedDomainEvent>
{
    public async Task Handle(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = $"Course {domainEvent.Course.Title} has been assigned to you. " +
                      $"Please check your tutor dashboard for more details.";

        var tutorEmail = await customerRepository.GetTutorEmail(domainEvent.TutorId);

        List<Notification> notifications =
        [
            Notification.Create(
                "Course Assigned",
                domainEvent.Course.Id.Value.ToString(),
                NotificationEnum.Course),
            Notification.Create(
                "Course was assigned to you",
                domainEvent.Course.Id.Value.ToString(),
                NotificationEnum.Course,
                currentUserService.UserId,
                domainEvent.TutorId.Value)
        ];
        await notificationRepository.InsertManyAsync(notifications, cancellationToken);

        if (await unitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            // Log error

            logger.LogError("Failed to save notification to database");

            return;
        }

        if (tutorEmail == null)
        {
            logger.LogWarning("Tutor email is null when assigning course: {CourseId}", domainEvent.Course.Id);

            return;
        }

        _ = emailSender.SendEmail(tutorEmail, "Course Assigned", message);
    }
}