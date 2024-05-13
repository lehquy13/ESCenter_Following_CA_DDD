using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Subscriptions.ValueObjects;

public class SubscriptionId : ValueObject
{
    public Guid Value { get; private set; }

    private SubscriptionId(Guid value)
    {
        Value = value;
    }

    public static SubscriptionId Create(Guid value = default)
    {
        return new(value == default ? Guid.NewGuid() : value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}