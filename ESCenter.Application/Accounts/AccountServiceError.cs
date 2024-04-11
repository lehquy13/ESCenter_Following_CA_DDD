using Matt.ResultObject;

namespace ESCenter.Application.Accounts;

public static class AccountServiceError
{
    public static Error NonExistUserError
        => new("UserNonExist", "This user doesn't exist!");

    public static Error FailToUpdateUserErrorWhileSavingChanges
        => new("FailToUpdateUserErrorWhileSavingChanges", "Fail to update user while saving changes!");

    public static Error FailToCreateUserErrorWhileSavingChanges
        => new("FailToCreateUserErrorWhileSavingChanges", "Fail to create user while saving changes!");
}