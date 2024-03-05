using Matt.ResultObject;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts;

public static class AuthenticationErrorMessages
{
    public static readonly Error UserNotFound = new("UserNotFound", "User not found");
    public static readonly Error LoginFailError = new("LoginFail", "Email or password is incorrect");
    public static readonly Error InvalidOtp = new("InvalidOtp", "Invalid OTP");

    public static readonly Error ResetPasswordFailWhileSavingChanges =
        new("ResetPasswordFailWhileSavingChanges", "Reset password fail while saving changes at AuthenticationService");

    public static readonly Error ResetPasswordFail =
        new("ResetPasswordFail", "Reset password fail at AuthenticationService");

    public static readonly Error ChangePasswordFail =
        new("ChangePasswordFail", "Change password fail at AuthenticationService");

    public static readonly Error ChangePasswordFailWhileSavingChanges = new("ChangePasswordFailWhileSavingChanges",
        "Change password fail while saving changes at AuthenticationService");

    public static readonly Error RegisterFail = new("RegisterFail", "Register fail at AuthenticationService");

    public static readonly Error CreateUserFailWhileSavingChanges = new("CreateUserFailWhileSavingChanges",
        "Create user fail while saving changes at AuthenticationService");

    public static readonly Error InvalidToken = new("InvalidToken", "Invalid token");
}