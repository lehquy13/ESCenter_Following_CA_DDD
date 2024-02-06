namespace ESCenter.Application.Contract.Commons.Primitives.Auditings;

public abstract class FullAuditedAggregateRootDto<TId> : EntityDto<TId>
    where TId : notnull
{
    public bool IsDeleted { get; set; }
    
    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }
    
    public DateTime? LastModificationTime { get; set; } = DateTime.Now;
    
    public long? LastModifierUserId { get; set; }
    
    public virtual DateTime CreationTime { get; set; }
    
    public virtual long? CreatorUserId { get; set; }
}