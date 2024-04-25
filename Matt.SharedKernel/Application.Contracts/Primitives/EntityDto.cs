namespace Matt.SharedKernel.Application.Contracts.Primitives;

public abstract class EntityDto<TId>
    where TId : notnull
{
    public TId Id { get; set; }

    protected EntityDto(TId id)
    {
        Id = id;
    }
#pragma warning disable CS8618

    protected EntityDto()
    {
    }

#pragma warning restore CS8618
}