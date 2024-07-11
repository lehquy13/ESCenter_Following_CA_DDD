using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.DomainServices.Errors;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.EventualConsistency;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.EventHandlers.CourseAssigned;

public class CreatePaymentCourseWhenAssignedDomainEventHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<TutorAssignedDomainEvent>
{
    public async Task Handle(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.Course.Status == Status.OnProgressing)
        {
            var course = domainEvent.Course;

            if (course.Status != Status.OnProgressing || course.TutorId == null)
            {
                throw new EventualConsistencyException(DomainServiceErrors.InvalidCourseStatusForPayment);
            }

            var payment = await paymentRepository.GetByCourseIdAsync(course.Id, cancellationToken);

            foreach (var payment1 in payment.Where(x => x.PaymentStatus == PaymentStatus.Pending))
            {
                payment1.Cancel();
            }

            var newPayment = Payment.Create(course.TutorId, course.Id, course.ChargeFee.Amount);

            await paymentRepository.InsertAsync(newPayment, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

public class CancelPaymentWhenUnAssignedDomainEventHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<TutorUnAssignedDomainEvent>
{
    public async Task Handle(TutorUnAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var course = domainEvent.Course;

        var payment = await paymentRepository.GetByCourseIdAsync(course.Id, cancellationToken);

        foreach (var payment1 in payment.Where(x => x.PaymentStatus == PaymentStatus.Pending))
        {
            payment1.Cancel();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}