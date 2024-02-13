using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class IdentityGuid : ValueObject
{
    public Guid Value { get; private set; }

    private IdentityGuid()
    {
    }

    public static IdentityGuid Create(Guid guid = default)
    {
        return new IdentityGuid()
        {
            Value = guid == default ? Guid.NewGuid() : guid
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}