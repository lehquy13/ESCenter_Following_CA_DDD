using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.ValueObjects;

public class TutorId : ValueObject
{
    public Guid Value { get; private set; }

    private TutorId()
    {
    }

    public static TutorId Create(Guid value = default)
    {
        return new()
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}