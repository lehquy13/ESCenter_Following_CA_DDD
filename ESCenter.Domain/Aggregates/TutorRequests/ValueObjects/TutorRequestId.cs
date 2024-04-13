using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;

public class TutorRequestId : ValueObject
{
    public Guid Value { get; private set; }
    
    private TutorRequestId(Guid value)
    {
        Value = value;
    }
    
    public static TutorRequestId Create(Guid value = default)
    {
        return new(value == default ? Guid.NewGuid() : value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}