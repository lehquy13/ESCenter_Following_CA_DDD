using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Admin.Application.EventHandlers;

public class CourseAssignedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Notification, int> notificationRepository
) : INotificationHandler<CourseAssignedDomainEvent>
{
    public async Task Handle(CourseAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = $"Course {domainEvent.Course.Title} has been assigned to you. " +
                      $"Please check your tutor dashboard for more details.";

        var tutorEmail = await customerRepository.GetTutorEmail(domainEvent.TutorId);

        emailSender.SendEmail(tutorEmail, "Course Assigned", message);

        var notification = Notification.Create(
            "Course Assigned ",
            domainEvent.Course.Id.Value.ToString(),
            NotificationEnum.Course);

        await notificationRepository.InsertAsync(notification, cancellationToken);
    }
}