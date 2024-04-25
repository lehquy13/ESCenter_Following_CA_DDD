using Matt.ResultObject;
using MediatR;

namespace Matt.SharedKernel.Application.Mediators.Queries;

public interface IQueryRequest<TResult> : IRequest<Result<TResult>> where TResult : class;
public interface IQueryRequest : IRequest<Result>;
