using Matt.ResultObject;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses;

public static class CourseRequestAppServiceErrors
{
    public static Error FailToCancelCourseRequestError 
        => new("CourseRequestAppServiceErrors.FailToCancelCourseRequestError", "Fail to cancel the course request");

    public static Error NonExistCourseError 
        => new("CourseRequestAppServiceErrors.NonExistCourseError", "The course does not exist");

    public static Error RequestedCourseError
        => new("CourseRequestAppServiceErrors.RequestedCourseError", "The course has been requested");

    public static Error FailToCreateCourseRequestError
        => new("CourseRequestAppServiceErrors.FailToCreateCourseRequestError", "Fail to create the course request");
}