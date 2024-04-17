using Matt.ResultObject;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs;

public static class StaffAppServiceError
{
    public static Error StaffNotFound => new Error("StaffNotFound", "Staff was not found");
    public static Error FailToUpdateStaffErrorWhileSavingChanges => new Error("FailToUpdateStaffErrorWhileSavingChanges", "Fail to update staff while saving changes");
    public static Error FailToCreateStaffErrorWhileSavingChanges => new Error("FailToCreateStaffErrorWhileSavingChanges", "Fail to create staff while saving changes");
    public static Error FailToDeleteStaffErrorWhileSavingChanges => new Error("FailToDeleteStaffErrorWhileSavingChanges", "Fail to delete staff while saving changes");
}