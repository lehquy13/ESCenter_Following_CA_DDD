using Matt.ResultObject;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles;

public static class TutorProfileAppServiceError
{
    public static Error NonExistTutorError => new("NonExistTutorError", "The tutor doesn't exist");
}