using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.CourseRequests.ValueObjects;

public class CourseRequestId : ValueObject
{
    public Guid Value { get; private set; }
    
    private CourseRequestId(Guid value)
    {
        Value = value;
    }
    
    public static CourseRequestId Create(Guid? value)
    {
        return new(value ?? Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}