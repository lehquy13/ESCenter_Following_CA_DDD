using ESCenter.Domain.Aggregates.Courses;
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

public class CourseConfirmedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CourseConfirmedDomainEvent>
{
    public async Task Handle(CourseConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.Course.TutorId is null)
        {
            throw new EventualConsistencyException(new Error("ConfirmedCourseWithoutTutor",
                "You cant confirm a course without a tutor"));
        }

        var tutor = await customerRepository.GetTutorByTutorId(domainEvent.Course.TutorId, cancellationToken);

        if (tutor is null)
        {
            throw new EventualConsistencyException(new Error("TutorNotFound", "Tutor not found"));
        }

        var message = $"Your course has been confirmed. \n. " +
                      $"Please contact for the learner to join your course.";

        await Task.WhenAll(
            notificationRepository.InsertAsync(Notification.Create(
                $"Course has been confirmed",
                domainEvent.Course.Id.Value.ToString(),
                NotificationEnum.Course), cancellationToken),
            emailSender.SendEmail(tutor.Email, "Course Confirmed", message));
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}public class ConfirmPaymentAfterCourseClosedDomainEventHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CourseConfirmedDomainEvent>
{
    public async Task Handle(CourseConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.Course.TutorId is null)
        {
            throw new EventualConsistencyException(new Error("ConfirmedCourseWithoutTutor",
                "You cant confirm a course without a tutor"));
        }

        // Check if payment exists using course id
        var payment = await paymentRepository.GetByCourseIdAsync(domainEvent.Course.Id, cancellationToken);

        if (payment is null)
        {
            throw new EventualConsistencyException(new Error("PaymentNotFound", "Payment not found"));
        }

        var paymentResult = payment.ConfirmPayment();

        if (paymentResult.IsFailure)
        {
            throw new EventualConsistencyException(paymentResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}