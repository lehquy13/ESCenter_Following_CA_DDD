namespace ESCenter.Application.Contracts.Commons.Primitives.Auditings;

public abstract class AuditedEntityDto<TId> : EntityDto<TId>
    where TId : notnull
{
  
    public DateTime? LastModificationTime { get; set; }
    public long? LastModifierUserId { get; set; }
    public  DateTime CreationTime { get; set; }
    public  long? CreatorUserId { get; set; }

    protected AuditedEntityDto(TId id) : base(id)
    {
    }

    protected AuditedEntityDto()
    {
        LastModificationTime = DateTime.Now;
        CreationTime = DateTime.Now;
    }
}