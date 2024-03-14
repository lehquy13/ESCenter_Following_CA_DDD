using Matt.ResultObject;

namespace ESCenter.Admin.Application.ServiceImpls.Users;

public static class UserAppServiceError
{
    public static Error NonExistUserError
        => new("UserNonExist", "This user doesn't exist!");

    public static Error FailToDeleteUserErrorWhileSavingChanges
        => new("FailToDeleteUserErrorWhileSavingChanges", "Fail to delete user while saving changes!");

    public static Error FailToAddOrResetDiscoveryErrorWhileSavingChanges =>
        new("FailToAddOrResetDiscoveryErrorWhileSavingChanges",
            "Fail to add or reset discovery while saving changes!");

    public static Error FailToUpdateUserErrorWhileSavingChanges =>
        new("FailToUpdateUserErrorWhileSavingChanges", "Fail to update user while saving changes!");

    public static Error FailToCreateUserErrorWhileSavingChanges =>
        new("FailToCreateUserErrorWhileSavingChanges", "Fail to create user while saving changes!");
}