using Matt.ResultObject;

namespace ESCenter.Domain.Aggregates.Users.Errors;

public static class UserNotFoundError
{
    public static Error UserNotFound()
    {
        return new Error("UserNotFound",
            "No user found");
    }

    public static Error UserNotFound(string userName)
    {
        return new Error("UserNotFound",
            $"No user found with username: {userName}");
    }
}