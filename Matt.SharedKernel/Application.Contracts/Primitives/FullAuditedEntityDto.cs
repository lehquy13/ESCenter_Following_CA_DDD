using Matt.Auditing;

namespace Matt.SharedKernel.Application.Contracts.Primitives;

public abstract class FullAuditedEntityDto<TId>
    : AuditedEntityDto<TId>, IFullAuditedObject
    where TId : notnull
{
    //Deletions
    public DateTime? DeletionTime { get; set; }

    public string? DeleterId { get; set; }

    public bool IsDeleted { get; set; }

    protected FullAuditedEntityDto(TId id) : base(id)
    {
    }

    protected FullAuditedEntityDto()
    {
    }
}