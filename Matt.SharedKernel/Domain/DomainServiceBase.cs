using Matt.SharedKernel.Domain.Interfaces;

namespace Matt.SharedKernel.Domain;

public abstract class DomainServiceBase(IAppLogger<DomainServiceBase> logger) : IDomainService
{
    protected readonly IAppLogger<DomainServiceBase> Logger = logger;
}