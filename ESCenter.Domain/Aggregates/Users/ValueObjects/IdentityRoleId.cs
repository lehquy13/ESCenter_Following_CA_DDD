using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class IdentityRoleId : ValueObject
{
    public int Value { get; private set; }
    
    private IdentityRoleId()
    {
    }
    
    public static IdentityRoleId Create(int value = 0)
    {
        return new()
        {
            Value = value
        };
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}