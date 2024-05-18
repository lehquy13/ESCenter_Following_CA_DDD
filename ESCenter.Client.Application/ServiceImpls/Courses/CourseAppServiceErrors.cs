using Matt.ResultObject;

namespace ESCenter.Client.Application.ServiceImpls.Courses;

public static class CourseAppServiceErrors
{
    public static Error CourseDoesNotExist
        => new("CourseAppServiceErrors.CourseDoesNotExist", "The class doesn't exist");

    public static Error IncorrectUserOfCourseError
        => new("CourseAppServiceErrors.IncorrectUserOfCourseError", "The user is not the owner of the class");

    public static Error DeleteCourseErrorWhileSavingChanges
        => new("CourseAppServiceErrors.DeleteCourseErrorWhileSavingChanges", "Error happens when class is deleting");

    public static Error CourseNotConfirmedError
        => new("CourseAppServiceErrors.CourseNotConfirmedError", "The class is not confirmed yet");

    public static Error TutorNotExistsError
        => new("CourseAppServiceErrors.TutorNotExistsError", "The tutor doesn't exist");

    public static Error FailToReviewCourse
        => new("CourseAppServiceErrors.FailToReviewCourse", "Fail to review the course");

    public static Error UpdateCourseFailedWhileSavingChanges
        => new("CourseAppServiceErrors.UpdateCourseFailedWhileSavingChanges",
            "Error happens when class is updating bc of saving changes");

    public static Error NonExistCourseRequestError
        => new("CourseAppServiceErrors.NonExistCourseRequestError", "The request doesn't exist");

    public static Error NonExistSubjectError 
        => new("CourseAppServiceErrors.NonExistSubjectError", "The subject doesn't exist");
}