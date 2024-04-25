using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class FullAuditedEntity<TId> : AuditedEntity<TId>, 
    IFullAuditedObject
    where TId : notnull
{
    public DateTime? DeletionTime { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public string? DeleterId { get; protected set; }

    protected FullAuditedEntity(TId id) : base(id)
    {
    }

    protected FullAuditedEntity()
    {
    }
}