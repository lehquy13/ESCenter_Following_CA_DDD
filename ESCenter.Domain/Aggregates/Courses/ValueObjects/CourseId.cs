using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class CourseId : ValueObject
{
    public Guid Value { get; private set; }
    
    private CourseId(Guid value)
    {
        Value = value;
    }
    
    public static CourseId Create(Guid? value)
    {
        return new(value ?? Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}