using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.Identities;

/// <summary>
/// Currently not used.
/// </summary>
public class IdentityUserRole : Entity<Guid>
{
    public IdentityGuid UserId { get; set; } = null!;

    public IdentityGuid RoleId { get; set; } = null!;
}