using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class IdentityRoleId : ValueObject
{
    public int Value { get; private set; }
    
    private IdentityRoleId(int value)
    {
        Value = value;
    }
    
    public static IdentityRoleId Create(int value = 0)
    {
        return new(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}