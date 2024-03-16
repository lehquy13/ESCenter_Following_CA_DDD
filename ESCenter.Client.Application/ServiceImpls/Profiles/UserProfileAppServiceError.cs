using Matt.ResultObject;

namespace ESCenter.Client.Application.ServiceImpls.Profiles;

public static class UserProfileAppServiceError
{
    public static Error NonExistUserError
        => new("UserNonExist", "This user doesn't exist!");

    public static Error FailToDeleteUserErrorWhileSavingChanges
        => new("FailToDeleteUserErrorWhileSavingChanges", "Fail to delete user while saving changes!");

    public static Error FailToAddOrResetDiscoveryErrorWhileSavingChanges =>
        new("FailToAddOrResetDiscoveryErrorWhileSavingChanges",
            "Fail to add or reset discovery while saving changes!");

    public static Error FailRegisteringAsTutorErrorWhileSavingChanges =>
        new("FailRegisteringAsTutorErrorWhileSavingChanges",
            "Fail to register as tutor while saving changes!");
}