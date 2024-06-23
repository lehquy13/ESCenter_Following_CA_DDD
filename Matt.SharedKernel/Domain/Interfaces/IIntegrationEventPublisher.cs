namespace Matt.SharedKernel.Domain.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent);
}
