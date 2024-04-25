using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace Matt.SharedKernel.Application.Mediators.Queries;

public abstract class QueryHandlerBase<TQuery, TResult>(
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : RequestHandlerBase(logger), IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQueryRequest<TResult>
    where TResult : class
{
    protected IMapper Mapper { get; } = mapper;

    public abstract Task<Result<TResult>> Handle(TQuery request, CancellationToken cancellationToken);
}

public abstract class QueryHandlerBase<TQuery>(
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : RequestHandlerBase(logger), IRequestHandler<TQuery, Result>
    where TQuery : IQueryRequest
{
    protected IMapper Mapper { get; } = mapper;
    public abstract Task<Result> Handle(TQuery query, CancellationToken cancellationToken);
}