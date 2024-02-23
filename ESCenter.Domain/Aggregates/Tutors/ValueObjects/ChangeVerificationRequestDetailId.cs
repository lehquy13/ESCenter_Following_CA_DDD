using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.ValueObjects;

public class ChangeVerificationRequestDetailId : ValueObject
{
    public Guid Value { get; private set; }

    private ChangeVerificationRequestDetailId()
    {
    }

    public static ChangeVerificationRequestDetailId Create(Guid value = default)
    {
        return new ChangeVerificationRequestDetailId
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}