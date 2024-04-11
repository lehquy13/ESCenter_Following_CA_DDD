using Matt.ResultObject;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses;

public static class CourseRequestAppServiceErrors
{
    public static Error NonExistCourseError
        => new("CourseRequestAppServiceErrors.NonExistCourseError", "The course does not exist");

    public static Error FailToCreateCourseRequestError
        => new("CourseRequestAppServiceErrors.FailToCreateCourseRequestError", "Fail to create the course request");
}