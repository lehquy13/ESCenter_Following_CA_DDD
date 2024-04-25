using Matt.ResultObject;
using MediatR;

namespace Matt.SharedKernel.Application.Mediators.Commands;

public interface ICommandRequest<TResult> : IRequest<Result<TResult>> where TResult : class;

public interface ICommandRequest : IRequest<Result>;