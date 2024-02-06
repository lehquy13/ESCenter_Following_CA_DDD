namespace ESCenter.Application.Contract.Commons.Primitives;

public abstract class AggregateRootDto<TId> : EntityDto<TId>
    where TId : notnull
{
    protected AggregateRootDto(TId id) : base(id)
    {
    }

    protected AggregateRootDto()
        : base()
    {
    }
}