using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class SessionDuration : ValueObject
{
    public int Value { get; private set; }

    public string Type { get; private set; }

    public string DisplayValue => $"{Value} minutes";

    public static SessionDuration Create(int value = 90, string? type = null)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(CourseError.InvalidMinuteValue);
        }

        return new(value, type ?? DurationType.Minute);
    }

    private SessionDuration(int value, string type)
    {
        Value = value;
        Type = type;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}