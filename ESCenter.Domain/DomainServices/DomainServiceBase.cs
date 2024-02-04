using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices;

public abstract class DomainServiceBase(IAppLogger<DomainServiceBase> logger) : IDomainService
{
    protected readonly IAppLogger<DomainServiceBase> Logger = logger;
}