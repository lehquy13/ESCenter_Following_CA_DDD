using Matt.ResultObject;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects;

public static class SubjectAppServiceError
{
    public static Error FailToAddSubjectErrorWhileSavingChanges
        => new("FailToAddSubjectErrorWhileSavingChanges", "Fail to add subject while saving changes");

    public static Error NonExistSubjectError => new("NonExistSubjectError", "Non-exist subject");
}