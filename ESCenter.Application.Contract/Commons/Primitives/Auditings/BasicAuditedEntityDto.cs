namespace ESCenter.Application.Contracts.Commons.Primitives.Auditings;

public abstract class BasicAuditedEntityDto<TId> : EntityDto<TId>
    where TId : notnull
{
  
    public DateTime? LastModificationTime { get; set; }
    public  DateTime CreationTime { get; set; }

    protected BasicAuditedEntityDto(TId id) : base(id)
    {
    }

    protected BasicAuditedEntityDto()
    {
        LastModificationTime = DateTime.Now;
        CreationTime = DateTime.Now;
    }
}