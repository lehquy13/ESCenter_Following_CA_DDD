using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.DiscoveryUsers;

public class DiscoveryUser : AggregateRoot<DiscoveryUserId>
{
    public DiscoveryId DiscoveryId { get; private set; } = null!;
    public IdentityGuid UserId { get; private set; } = null!;

    private DiscoveryUser()
    {
    }

    public static DiscoveryUser Create(DiscoveryId discoveryId, IdentityGuid userId)
    {
        return new DiscoveryUser
        {
            DiscoveryId = discoveryId,
            UserId = userId
        };
    }
}