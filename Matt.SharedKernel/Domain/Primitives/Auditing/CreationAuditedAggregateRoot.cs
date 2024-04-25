using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class CreationAuditedAggregateRoot<TId> : AggregateRoot<TId>, ICreationAuditedObject
    where TId : notnull
{
    public DateTime CreationTime { get; protected set; }

    public string? CreatorId { get; protected set; }

    protected CreationAuditedAggregateRoot()
    {
    }

    protected CreationAuditedAggregateRoot(TId id)
        : base(id)
    {
    }
}