using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Specifications.Users;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.DomainServices;

public class SubscribeDomainService(
    IRepository<Subscriber, int> subscriberRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<SubscribeDomainService> logger)
    : DomainServiceBase(logger), ISubscribeDomainService
{
    public async Task<Result> Subscribe(string mail)
    {
        var user = await userRepository.GetAsync(new UserByEmailSpec(mail));
        
        if (user is null)
        {
            return Result.Fail("User not found");
        }
        
        var subscriber = await subscriberRepository.GetAsync(new SubscriberByUserIdSpec(user.Id));
        
        if (subscriber is not null)
        {
            return Result.Fail("Subscriber already exists");
        }
        
        subscriber = Subscriber.Create(user.Id);
        
        await subscriberRepository.InsertAsync(subscriber);
        
        return Result.Success();
    }

    public async Task<Result> UnSubscribe(string mail)
    {
        var user = await userRepository.GetAsync(new UserByEmailSpec(mail));
        
        if (user is null)
        {
            return Result.Fail("User not found");
        }
        
        var subscriber = await subscriberRepository.GetAsync(new SubscriberByUserIdSpec(user.Id));
        
        if (subscriber is null)
        {
            return Result.Fail("Subscriber not found");
        }
        
        await subscriberRepository.RemoveAsync(subscriber.Id);
        
        return Result.Success();
    }
}