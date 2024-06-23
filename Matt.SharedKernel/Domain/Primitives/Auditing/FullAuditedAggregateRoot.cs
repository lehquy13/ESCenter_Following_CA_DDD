using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class FullAuditedAggregateRoot<TId> : AuditedAggregateRoot<TId>,
    IFullAuditedObject where TId : notnull
{
    public bool IsDeleted { get; protected set; }

    public DateTime? DeletionTime { get; protected set; }

    public string? DeleterId { get; protected set; }

    protected FullAuditedAggregateRoot(TId id) : base(id)
    {
    }
    
    protected FullAuditedAggregateRoot()
    {
    }
}