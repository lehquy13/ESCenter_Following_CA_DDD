
using Matt.ResultObject;

namespace ESCenter.Domain.Aggregates.Courses.Errors;

public class RequestError
{
    public static Error NonExistRequestError =>
        new Error("NonExistRequestError", "Tutor has not requested this course yet!");
    
    public static Error ExistedRequestError =>
        new Error("ExistedRequestError", "Tutor has already requested this course!");
}
