using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.ValueObjects;

public class TutorMajorId : ValueObject
{
    public Guid Value { get; private set; }
    
    private TutorMajorId(Guid value)
    {
        Value = value;
    }
    
    public static TutorMajorId Create(Guid? value)
    {
        return new(value ?? Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}