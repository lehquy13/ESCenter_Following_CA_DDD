namespace Matt.SharedKernel.Domain.Interfaces;

public interface IHasDomainEvents
{
    List<IDomainEvent> PopDomainEvents();
}