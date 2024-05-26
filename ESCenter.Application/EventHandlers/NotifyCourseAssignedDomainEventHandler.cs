using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers;

public class NotifyCourseAssignedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<TutorAssignedDomainEvent>
{
    public async Task Handle(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = $"Course {domainEvent.Course.Title} has been assigned to you. " +
                      $"Please check your tutor dashboard for more details.";

        var tutorEmail = await customerRepository.GetTutorEmail(domainEvent.TutorId);

        var notification = Notification.Create(
            "Course Assigned",
            domainEvent.Course.Id.Value.ToString(),
            NotificationEnum.Course);

        await notificationRepository.InsertAsync(notification, cancellationToken);

        if (await unitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return;
        }

        if (tutorEmail == null)
            return;

        _ = emailSender.SendEmail(tutorEmail, "Course Assigned", message);
    }
}