using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Payment;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.EventualConsistency;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.EventHandlers.CourseCanceled;

public class RefundPaymentWhenCourseCancelDomainEventHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CanceledAndRefundedCourseEvent>
{
    public async Task Handle(CanceledAndRefundedCourseEvent domainEvent, CancellationToken cancellationToken)
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

        var paymentResult = payment.Refund();

        if (paymentResult.IsFailure)
        {
            throw new EventualConsistencyException(paymentResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}