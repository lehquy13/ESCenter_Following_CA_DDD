namespace Matt.SharedKernel.Application.Authorizations;

public static partial class Permission
{
    public static class Staff
    {
        public const string Set = "set:staff";
        public const string Get = "get:staff";
        public const string Dismiss = "dismiss:staff";
        public const string Delete = "delete:staff";
    }
}