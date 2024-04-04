using ESCenter.Domain.Aggregates.Courses.Errors;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class SessionPerWeek : ValueObject
{
    public int Value { get; private set; }
    
    public static SessionPerWeek Create(int value = 0)
    {
        if (value <= 0 || value > 7)
        {
            throw new ArgumentOutOfRangeException(CourseDomainError.InvalidSectionRange);
        }
        return new SessionPerWeek
        {
            Value = value
        };
    }
    
    private SessionPerWeek()
    {
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}