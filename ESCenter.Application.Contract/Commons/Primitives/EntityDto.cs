using ESCenter.Application.Contract.Commons.Primitives.Interfaces;

namespace ESCenter.Application.Contract.Commons.Primitives;

public abstract class EntityDto<TId> : IEntity<TId>, IEquatable<EntityDto<TId>>
    where TId : notnull
{
    public TId Id { get; set; }

    protected EntityDto(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        return obj is EntityDto<TId> entity && Id.Equals(entity.Id);
    }

    public bool Equals(EntityDto<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(EntityDto<TId>? left, EntityDto<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EntityDto<TId>? left, EntityDto<TId>? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

#pragma warning disable CS8618

    protected EntityDto() { }

#pragma warning restore CS8618
}