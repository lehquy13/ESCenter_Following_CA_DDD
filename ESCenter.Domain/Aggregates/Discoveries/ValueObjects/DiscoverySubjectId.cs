using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries.ValueObjects;

public class DiscoverySubjectId : ValueObject
{
    public Guid Value { get; private set; }
    
    private DiscoverySubjectId()
    {
    }
    
    public static DiscoverySubjectId Create(Guid value = default)
    {
        return new DiscoverySubjectId { Value = value == default ? Guid.NewGuid() : value };
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}