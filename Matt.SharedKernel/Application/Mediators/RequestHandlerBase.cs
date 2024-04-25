using Matt.SharedKernel.Domain.Interfaces;

namespace Matt.SharedKernel.Application.Mediators;

public abstract class RequestHandlerBase( IAppLogger<RequestHandlerBase> logger)
{
    protected readonly IAppLogger<RequestHandlerBase> Logger = logger;
}