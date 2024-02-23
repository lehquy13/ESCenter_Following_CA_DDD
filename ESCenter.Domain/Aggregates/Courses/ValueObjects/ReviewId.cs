using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class ReviewId : ValueObject
{
    public Guid Value { get; private set; }
    
    private ReviewId()
    {
    }
    
    public static ReviewId Create(Guid value = default)
    {
        return new ReviewId { Value = value == default ? Guid.NewGuid() : value };
    }

    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}