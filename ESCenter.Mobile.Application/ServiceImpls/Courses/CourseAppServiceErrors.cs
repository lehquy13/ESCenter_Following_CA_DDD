using Matt.ResultObject;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses;

public static class CourseAppServiceErrors
{
    public static Error CourseDoesNotExist
        => new("CourseAppServiceErrors.CourseDoesNotExist", "The class doesn't exist");

    public static Error TutorNotExistsError
        => new("CourseAppServiceErrors.TutorNotExistsError", "The tutor doesn't exist");

    public static Error TutorDoesNotExist => new("CourseAppServiceErrors.TutorDoesNotExist", "The tutor doesn't exist");

    public static Error CourseHasNoTutor => new("CourseAppServiceErrors.CourseHasNoTutor", "The course has no tutor");
}