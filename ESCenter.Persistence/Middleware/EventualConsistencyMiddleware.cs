using ESCenter.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.EventualConsistency;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ESCenter.Persistence.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, AppDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value) &&
                    value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (EventualConsistencyException eventualConsistencyException)
            {
                // Handle eventual consistency exceptions
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await next(context);
    }
}