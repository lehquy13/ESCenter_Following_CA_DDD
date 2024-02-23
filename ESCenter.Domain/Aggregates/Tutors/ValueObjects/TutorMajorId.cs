using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.ValueObjects;

public class TutorMajorId : ValueObject
{
    public Guid Value { get; private set; }

    private TutorMajorId()
    {
    }

    public static TutorMajorId Create(Guid value = default)
    {
        return new TutorMajorId
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}