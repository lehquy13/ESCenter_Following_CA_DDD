using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Payment;

public class PaymentId : ValueObject
{
    public Guid Value { get; private init; }

    private PaymentId()
    {
    }

    public static PaymentId Create(Guid value = default)
    {
        return new PaymentId
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}