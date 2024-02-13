using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Subjects.ValueObjects;

public class SubjectId : ValueObject
{
    public int Value { get; private set; } = 0;
    
    private SubjectId()
    {
    }
    
    public static SubjectId Create(int value = 0)
    {
        return new()
        {
            Value = value
        };
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}