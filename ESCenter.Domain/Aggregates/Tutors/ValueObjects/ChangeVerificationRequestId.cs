using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.ValueObjects;

public class ChangeVerificationRequestId : ValueObject
{
    public Guid Value { get; private set; }

    private ChangeVerificationRequestId()
    {
    }

    public static ChangeVerificationRequestId Create(Guid value = default)
    {
        return new ChangeVerificationRequestId
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}