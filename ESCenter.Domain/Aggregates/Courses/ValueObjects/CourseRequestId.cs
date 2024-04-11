using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class CourseRequestId : ValueObject
{
    public Guid Value { get; private set; }

    private CourseRequestId(Guid value)
    {
        Value = value;
    }

    public static CourseRequestId Create(Guid value = default)
    {
        return new(value == Guid.Empty ? Guid.NewGuid() : value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}