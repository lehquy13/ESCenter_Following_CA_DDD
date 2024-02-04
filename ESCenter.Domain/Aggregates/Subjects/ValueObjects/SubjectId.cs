using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Subjects.ValueObjects;

public class SubjectId : ValueObject
{
    public int Value { get; private set; }
    
    private SubjectId(int value)
    {
        Value = value;
    }
    
    public static SubjectId Create(int value = 0)
    {
        return new(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}