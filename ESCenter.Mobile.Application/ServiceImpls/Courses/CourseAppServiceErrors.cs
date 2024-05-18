using Matt.ResultObject;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses;

public static class CourseAppServiceErrors
{
    public static Error CourseDoesNotExist
        => new("CourseAppServiceErrors.CourseDoesNotExist", "The class doesn't exist");
    public static Error TutorNotExistsError
        => new("CourseAppServiceErrors.TutorNotExistsError", "The tutor doesn't exist");
    public static Error TutorDoesNotExist => new("CourseAppServiceErrors.TutorDoesNotExist", "The tutor doesn't exist");
    public static Error CourseHasNoTutor => new("CourseAppServiceErrors.CourseHasNoTutor", "The course purchased but has no tutor");
    public static Error NonExistSubjectError => new("CourseAppServiceErrors.NonExistSubjectError", "The subject doesn't exist");
    public static Error NonExistCourseRequestError => new("CourseAppServiceErrors.NonExistCourseRequestError", "The course request doesn't exist");
}