using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Specifications.Customers;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.DomainServices;

public class SubscribeDomainService(
    IRepository<Subscriber, int> subscriberRepository,
    IAppLogger<SubscribeDomainService> logger)
    : DomainServiceBase(logger), ISubscribeDomainService
{
    public async Task<Result> Subscribe(string mail)
    {
        var subscriber = await subscriberRepository.GetAsync(new SubscriberByMailSpec(mail));

        if (subscriber is not null)
        {
            return Result.Fail("Subscriber already exists");
        }

        subscriber = Subscriber.Create(mail);

        await subscriberRepository.InsertAsync(subscriber);

        return Result.Success();
    }

    public async Task<Result> UnSubscribe(string mail)
    {
        var subscriber = await subscriberRepository.GetAsync(new SubscriberByMailSpec(mail));

        if (subscriber is null)
        {
            return Result.Fail("Subscriber not found");
        }

        await subscriberRepository.RemoveAsync(subscriber.Id);

        return Result.Success();
    }
}