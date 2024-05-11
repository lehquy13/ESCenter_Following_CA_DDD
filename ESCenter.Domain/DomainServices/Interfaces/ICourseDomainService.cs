using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices.Interfaces;

public interface ICourseDomainService : IDomainService
{
    Task<Result> RequestCourse(CourseId courseId, CustomerId customerId);

    Task<Result> CancelCourseRequest(
        CourseId commandCourseId,
        CourseRequestId commandCourseRequestId,
        string commandDescription);

    Task<Result> PurchaseCourse(
        CourseId courseId,
        CustomerId customerId);

    Task<Result> ReviewCourse(CourseId courseId, short rate, string detail, CustomerId customerId);
}