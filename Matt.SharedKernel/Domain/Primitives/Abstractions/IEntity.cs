namespace Matt.SharedKernel.Domain.Primitives.Abstractions;

public interface IEntity<TId> where TId : notnull
{
    TId Id { get; }
}