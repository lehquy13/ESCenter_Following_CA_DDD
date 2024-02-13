using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class CourseId : ValueObject
{
    public Guid Value { get; private set; }
    
    private CourseId()
    {
    }
    
    public static CourseId Create(Guid value = default)
    {
        return new CourseId { Value = value == default ? Guid.NewGuid() : value };
    }

    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}