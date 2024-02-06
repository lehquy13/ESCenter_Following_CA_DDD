using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contract.Commons;

public abstract class BasicAuditedEntityDto<TId> : EntityDto<TId>
    where TId : notnull
{
    public DateTime? LastModificationTime { get; set; } = DateTime.Now;
    public  DateTime CreationTime { get; set; } = DateTime.Now;
}