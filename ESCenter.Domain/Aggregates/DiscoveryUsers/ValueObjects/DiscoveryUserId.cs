using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.DiscoveryUsers.ValueObjects;

public class DiscoveryUserId : ValueObject
{
    public int Value { get; private init; }

    private DiscoveryUserId()
    {
    }

    public static DiscoveryUserId Create(int? value)
    {
        return new DiscoveryUserId { Value = value ?? 0 };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}