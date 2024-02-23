using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries.ValueObjects;

public class DiscoveryId : ValueObject
{
    public Guid Value { get; private set; }
    
    private DiscoveryId()
    {
    }
    
    public static DiscoveryId Create(Guid value = default)
    {
        return new DiscoveryId { Value = value == default ? Guid.NewGuid() : value };
    }

    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}