using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Payment;

public class Payment : FullAuditedAggregateRoot<PaymentId>
{
    public TutorId TutorId { get; private set; } = null!;
    public CourseId CourseId { get; private set; } = null!;
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;
    public string Code { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }

    private Payment()
    {
    }

    public static Payment Create(TutorId tutorId, CourseId courseId, decimal amount)
    {
        return new Payment
        {
            Amount = amount,
            TutorId = tutorId,
            CourseId = courseId,
            PaymentStatus = PaymentStatus.Pending,
            Code = RandomString(8)
        };
    }

    private static readonly Random Random = new Random();

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public void Cancel()
    {
        PaymentStatus = PaymentStatus.Canceled;
    }

    public Result SetTutorPaid()
    {
        if (PaymentStatus != PaymentStatus.Pending)
        {
            return Result.Fail("Payment not pending");
        }

        PaymentStatus = PaymentStatus.UnverifiedPayment;

        DomainEvents.Add(new TutorPaidDomainEvent(this));

        return Result.Success();
    }

    public Result ConfirmPayment()
    {
        if (PaymentStatus != PaymentStatus.Canceled)
        {
            return Result.Fail("Payment not paid");
        }

        PaymentStatus = PaymentStatus.Completed;

        return Result.Success();
    }

    public Result Refund()
    {
        if (PaymentStatus != PaymentStatus.Completed)
        {
            return Result.Fail("Payment not completed");
        }

        PaymentStatus = PaymentStatus.Refunded;

        return Result.Success();
    }
}

// TODO: add domain event handler
public record TutorPaidDomainEvent(Payment Payment) : IDomainEvent;

public enum PaymentStatus
{
    Pending,
    Completed,
    Canceled,
    UnverifiedPayment,
    Refunded
}