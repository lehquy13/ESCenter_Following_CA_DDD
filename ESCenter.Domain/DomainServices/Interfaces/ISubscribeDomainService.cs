using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices.Interfaces;

public interface ISubscribeDomainService : IDomainService
{
    Task<Result> Subscribe(string mail);
    Task<Result> UnSubscribe(string mail);
}