using Matt.ResultObject;

namespace ESCenter.Domain.DomainServices.Errors;

public static class DomainServiceErrors
{
    public static readonly Error UserNotFound = new("UserNotFound", "User not found");
    public static readonly Error InvalidPassword = new("InvalidPassword", "Invalid password");
    public const string ChangePasswordFailWhileSavingChanges = "Change password fail while saving changes";
    public const string CreateUserFailWhileSavingChanges = "Create user fail while saving changes";
    public static readonly Error UserAlreadyExistDomainError = new("UserAlreadyExistDomainError", "User already exist");
    public static readonly Error RoleNotFoundDomainError = new("RoleNotFoundDomainError", "Role not found");
    public static readonly Error RoleNameIsExistDomainError = new("RoleNameIsExistDomainError", "Role name is exist");
    public static readonly Error InvalidOtp = new("InvalidOtp", "Invalid otp");
    public static readonly Error OtpExpired = new("OtpExpired", "Otp expired");
    public static readonly Error AlreadyTutorError = new("AlreadyTutorError", "User have already been a tutor");
    public static Error FailRegisteringAsTutorErrorWhileSavingChanges(string message)
        => new("FailRegisteringTutorErrorWhileSavingChanges",
            $"Fail to register tutor while saving changes! {message}");
}