namespace ESCenter.Application.Contract.Commons.Primitives.Auditings;

public abstract class BasicAuditedEntityDto<TId> : EntityDto<TId>
    where TId : notnull
{
    public DateTime? LastModificationTime { get; set; } = DateTime.Now;
    public  DateTime CreationTime { get; set; } = DateTime.Now;
}