using Matt.Auditing;

namespace Matt.SharedKernel.Application.Contracts.Primitives;

public abstract class AuditedEntityDto<TId>
    : CreationAuditedEntityDto<TId>, IAuditedObject
    where TId : notnull
{
    //Modifications
    public DateTime? LastModificationTime { get; set; }

    public string? LastModifierId { get; set; }

    protected AuditedEntityDto(TId id) : base(id)
    {
        LastModificationTime = DateTime.Now.ToLocalTime();
    }

    protected AuditedEntityDto()
    {
    }
}