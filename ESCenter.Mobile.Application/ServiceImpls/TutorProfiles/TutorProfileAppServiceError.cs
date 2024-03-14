using Matt.ResultObject;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles;

public static class TutorProfileAppServiceError
{
    public static Error NonExistTutorError => new("NonExistTutorError", "The tutor doesn't exist");
    public static Error UnauthorizedError => new("UnauthorizedError", "Unauthorized");
}