using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.Identities;

public class IdentityRole : AggregateRoot<IdentityRoleId>
{
    public const int Admin = 0;
    public const int Tutor = 1;
    public const int Learner = 2;
    
    public string Name { get; private set; } = string.Empty;

    private IdentityRole()
    {
    }
    
    public static IdentityRole Create(string name)
    {
        return new IdentityRole
        {
            Id = IdentityRoleId.Create(),
            Name = name
        };
    }
}