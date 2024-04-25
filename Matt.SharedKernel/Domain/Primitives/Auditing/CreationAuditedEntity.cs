using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class CreationAuditedEntity<TId> : Entity<TId>, ICreationAuditedObject where TId : notnull
{
    public DateTime CreationTime { get; protected set; }

    public string? CreatorId { get; protected set; }

    protected CreationAuditedEntity()
    {
        CreationTime = DateTime.Now.ToLocalTime();
    }

    protected CreationAuditedEntity(TId id)
        : base(id)
    {
    }
}