using Matt.ResultObject;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts;

public static class AccountServiceError
{
    public const string FailToGetTutorProfileWithException = "Fail to get tutor profile with exception: ";

    public static string FailToGetUserProfileWithException = "Fail to get user profile";

    public const string NonExistTutorError = "This tutor profile doesn't exist!";

    public static Error AlreadyTutorError
        => new("TutorAlready", "This user is already a tutor!");

    public static Error NonExistUserError
        => new("UserNonExist", "This user doesn't exist!");

    public static string FailRegisteringAsTutorError => "Fail to register tutor at RegisterAsTutorCommandHandler!";

    public static Error FailRegisteringAsTutorErrorWhileSavingChanges =>
        new("FailRegisteringTutorErrorWhileSavingChanges", "Fail to register tutor while saving changes!");

    public static Error FailRegisteringAsTutorErrorWithException
        => new("FailRegisteringAsTutorErrorWithException", "Fail to register as tutor with exception: ");

    public static Error FailToUpdateTutorErrorWhileSavingChanges
        => new("FailToUpdateTutorErrorWhileSavingChanges", "Fail to update tutor while saving changes!");

    public static Error FailToUpdateUserErrorWhileSavingChanges
        => new("FailToUpdateUserErrorWhileSavingChanges", "Fail to update user while saving changes!");

    public static Error FailToCreateUserErrorWhileSavingChanges
        => new("FailToCreateUserErrorWhileSavingChanges", "Fail to create user while saving changes!");

    public static Error UnauthorizedError 
        => new("Unauthorized", "Unauthorized user!");

    public static Error FailToUpdateTutorError(string innerExceptionMessage)
    {
        return new("FailToUpdateTutorError", $"Fail to update tutor! {innerExceptionMessage}");
    }
}