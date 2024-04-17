using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Staff;

public class StaffId : ValueObject
{
    public Guid Value { get; private set; }

    private StaffId()
    {
    }

    public static StaffId Create(Guid value = default)
    {
        return new StaffId
        {
            Value = value == default ? Guid.NewGuid() : value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}