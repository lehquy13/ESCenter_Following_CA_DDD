using Matt.ResultObject;

namespace ESCenter.Client.Application.ServiceImpls.Subjects;

public static class SubjectAppServiceError
{
    public static readonly Error SubjectAlreadyExists = new("SubjectAlreadyExists", "Subject already exists");

    public static Error FailToAddSubjectErrorWhileSavingChanges
        => new("FailToAddSubjectErrorWhileSavingChanges", "Fail to add subject while saving changes");

    public static Error NonExistSubjectError => new("NonExistSubjectError", "Non-exist subject");
}