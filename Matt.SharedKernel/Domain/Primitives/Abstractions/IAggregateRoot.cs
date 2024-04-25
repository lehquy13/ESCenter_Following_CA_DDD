using Matt.SharedKernel.Domain.Interfaces;

namespace Matt.SharedKernel.Domain.Primitives.Abstractions;

public interface IAggregateRoot<TId> : IEntity<TId>, IHasDomainEvents where TId : notnull;