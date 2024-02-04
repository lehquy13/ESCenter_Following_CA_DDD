using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries.ValueObjects;

public class DiscoveryId : ValueObject
{
    public int Value { get; private init; }

    private DiscoveryId()
    {
    }

    public static DiscoveryId Create(int? value)
    {
        return new DiscoveryId { Value = value ?? 0 };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}