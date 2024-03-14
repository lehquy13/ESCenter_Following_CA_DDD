using Matt.ResultObject;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles;

public static class TutorProfileAppServiceError
{
    public static Error NonExistTutorError => new("NonExistTutorError", "The tutor doesn't exist");
    public static Error UnauthorizedError => new("UnauthorizedError", "You are not authorized to perform this action");
}