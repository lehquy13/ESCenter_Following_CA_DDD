using Matt.ResultObject;

namespace ESCenter.Domain.Aggregates.Courses.Errors;

public static class CourseDomainError
{
    public static readonly string InvalidSectionRange = "Invalid section range! It must be between 1 and 7!";
    public static readonly string InvalidMinuteValue = "Invalid minute value! It must be equal or more than 60!";
    public static readonly string InvalidReviewRate = "Invalid review rate! It must be between 1 and 5!";
    public static readonly string InvalidDetailLength = "Invalid detail length! It must be less than 500 characters!";

    public static Error NonExistCourseError => new Error("NonExistCourseError", "This course doesn't exist!");
    public static Error UnAvailableClassError => new Error("UnAvailableClassError", "This course isn't available!");
    public static Error NonExistSubjectError => new Error("NonExistSubjectError", "This subject doesn't exist!");

    public static Error IncorrectUserOfCourseError =>
        new("IncorrectUserOfCourseError", "This user isn't the owner of this course!");

    public static Error NonExistCourseRequestError => new("NonExistCourseRequestError", "This request doesn't exist!");

    public static Error RequestedCourseError => new("RequestedCourseError", "Course was requested!");

    public static Error InvalidStatusForReview { get; } = new("ReviewFailError",
        "This class isn't available for review due to not being confirmed!");

    public static Error InsufficientLearningDayForReview { get; } = new("ReviewFailError",
        "This class can't be reviewed due to insufficient learning days!");

    public static Error AlreadyReviewedErrorMessage { get; } = new("ReviewFailError", "This class has been reviewed!");
    public static Error CourseIsNotOnGoing { get; } = new("CourseIsNotOnGoing", "This course isn't on going!");

    public static Error MaxCourseRequestError { get; } = new("MaxCourseRequestError",
        "This course has reached the maximum number of requests!");

    public static Error CourseUnavailable { get; } = new("CourseConfirmed", "This course has been confirmed!");
    public static Error NonExistTutorError { get; } = new("NonExistTutorError", "This tutor doesn't exist!");

    public static Error InvalidStatusForAssignTutor { get; } = new("InvalidStatusForAssignTutor",
        "This course isn't available for assigning tutor!");

    public static Error CourseUnavailableForConfirmation { get; } = new("CourseUnavailableForConfirmation",
        "This course isn't available for confirmation!");
}