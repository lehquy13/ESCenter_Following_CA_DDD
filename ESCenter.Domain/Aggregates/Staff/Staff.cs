using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Staff;

public class Staff : AggregateRoot<StaffId>
{
    private Staff()
    {
    }

    public static Staff Create(Guid userId)
    {
        return new Staff
        {
            Id = StaffId.Create(userId),
        };
    }
}