using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers;

public class CoursePurchasedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CoursePurchasedDomainEvent>
{
    public async Task Handle(CoursePurchasedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = $"You have purchased course {domainEvent.Course.Title}. " +
                      $"Please wait for the administrator to confirm your purchase.";

        var tutorEmail = await customerRepository.GetTutorEmail(domainEvent.TutorId);

        if (tutorEmail is null)
        {
            return;
        }

        _ = emailSender.SendEmail(tutorEmail, "Course Purchased", message);

        var notification = Notification.Create(
            $"A course has been purchased",
            domainEvent.Course.Id.Value.ToString(),
            NotificationEnum.Course);

        await notificationRepository.InsertAsync(notification, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}