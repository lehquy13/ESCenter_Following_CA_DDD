using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.EventualConsistency;
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
) : INotificationHandler<TutorPaidDomainEvent>
{
    public async Task Handle(TutorPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var tutor = await customerRepository.GetTutorByTutorId(domainEvent.Payment.TutorId, cancellationToken);

        if (tutor is null)
        {
            throw new EventualConsistencyException(new Error("TutorNotFound", "Tutor not found"));
        }

        var message = $"You have purchased our course. \n. " +
                      $"PaymentId: {domainEvent.Payment.Id.Value}\n" +
                      $"Please wait for the administrator to confirm your purchase.";

        var tutorEmail = await customerRepository.GetTutorEmail(domainEvent.Payment.TutorId);

        if (tutorEmail is null)
        {
            return;
        }

        _ = emailSender.SendEmail(tutorEmail, "Course Purchased", message);

        var notification = Notification.Create(
            $"A course has been purchased",
            domainEvent.Payment.CourseId.Value.ToString(),
            NotificationEnum.Course);

        await notificationRepository.InsertAsync(notification, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}