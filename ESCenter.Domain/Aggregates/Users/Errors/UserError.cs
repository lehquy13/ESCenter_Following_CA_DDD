using Matt.ResultObject;

namespace ESCenter.Domain.Aggregates.Users.Errors;

public class UserError
{
    public const string NonExistTutorVerificationInfo = "This tutor verification info doesn't exist!";


    public const string NonExistChangeVerificationRequest = "This change verification request doesn't exist!";
    public const string FailToUpdateChangeVerificationRequest = "Fail to update change verification request!";
    public const string AlreadyTutorError = "This user is already a tutor!";
    public const string FailToRequestTutor = "Fail to request tutor!";
    public static Error NonExistUserError => new("NonExistUserError", "This user doesn't exist!");
    public static Error CantGetUserProfile => new("GetUserProfileFail", "Fail to get user profile at Account Service!");
    public static Error NonExistTutorError => new ("NonExistTutorError", "This tutor doesn't exist! ");

    public static string FailToUpdateUserError(string email)
    {
        return $"Fail to update user with email + {email}!";
    }

    public static string FailTCreateUserError(string email)
    {
        return $"Fail to create user with email + {email}!";
    }

    public static string FailToUpdateTutorError(string email)
    {
        return $"Fail to update tutor with id + {email}!";
    }

    public static string FailTCreateTutorError(string email)
    {
        return $"Fail to create tutor with email + {email}!";
    }

    public static string FailTCreateOrUpdateUserError(string email, string message)
    {
        return $"Fail to create user with email + {email} because {message}!";
    }

    public static string FailToDeleteUserError(string userEmail)
    {
        return $"Fail to delete user with email + {userEmail}!";
    }
}