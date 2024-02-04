using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class IdentityGuid : ValueObject
{
    public Guid Value { get; }

    private IdentityGuid(Guid value)
    {
        Value = value;
    }

    private IdentityGuid()
    {
    }

    public static IdentityGuid Create()
    {
        return new IdentityGuid(Guid.NewGuid());
    }

    public static IdentityGuid Create(Guid guid)
    {
        return new IdentityGuid(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}