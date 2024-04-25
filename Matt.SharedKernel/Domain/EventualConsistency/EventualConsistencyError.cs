using Matt.ResultObject;

namespace Matt.SharedKernel.Domain.EventualConsistency;

public static class EventualConsistencyError
{
    public const string EventualConsistencyType = "EventualConsistency";

    public static Error From(string code, string description)
    {
        return new Error(EventualConsistencyType,  description);
    }
}