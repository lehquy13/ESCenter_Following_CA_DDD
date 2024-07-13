using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class AuditedAggregateRoot<TId> : AggregateRoot<TId>, IAuditedObject
    where TId : notnull
{
    //Creation
    public DateTime CreationTime { get; protected set; }

    public string? CreatorId { get; protected set; }

    //Modification
    public DateTime? LastModificationTime { get; protected set;}

    public string? LastModifierId { get; protected set; }

    protected AuditedAggregateRoot(TId id) : base(id)
    {
        CreationTime = DateTime.Now.ToLocalTime();
        LastModificationTime = DateTime.Now.ToLocalTime();
    }

    protected AuditedAggregateRoot()
    {
    }
}