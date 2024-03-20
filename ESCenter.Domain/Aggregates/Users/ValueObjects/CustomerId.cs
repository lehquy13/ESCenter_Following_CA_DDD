using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Users.ValueObjects;

public class CustomerId : ValueObject
{
    public Guid Value { get; private set; }

    private CustomerId()
    {
    }

    public static CustomerId Create(Guid guid = default)
    {
        return new CustomerId()
        {
            Value = guid == default ? Guid.NewGuid() : guid
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}