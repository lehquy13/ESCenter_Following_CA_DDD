namespace ESCenter.Application.Contract.Commons.Primitives.Interfaces;

public interface IEntity<TId> where TId : notnull
{
    TId Id { get; }
}