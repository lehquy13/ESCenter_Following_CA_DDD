using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Specifications.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices;

public class CourseDomainService(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    IAppLogger<CourseDomainService> logger)
    : DomainServiceBase(logger), ICourseDomainService
{
    public async Task<Result> RequestCourse(CourseId courseId, CustomerId customerId)
    {
        var course = await courseRepository.GetAsync(courseId);

        if (course is null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        var tutor = await tutorRepository.GetAsync(new TutorByCustomerIdSpec(customerId));

        if (tutor is null)
        {
            return Result.Fail(CourseDomainError.NonExistTutorError);
        }

        var courseRequestToCreate = CourseRequest.Create(
            tutor.Id,
            course.Id,
            string.Empty
        );

        var result = course.Request(courseRequestToCreate);

        return result;
    }

    public async Task<Result> CancelCourseRequest(CourseId commandCourseId, CourseRequestId commandCourseRequestId,
        string commandDescription)
    {
        var course = await courseRepository.GetAsync(commandCourseId);

        if (course is null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        var courseRequest =
            course.CourseRequests.FirstOrDefault(x => x.Id == commandCourseRequestId);

        if (courseRequest is null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseRequestError);
        }

        courseRequest.Cancel(commandDescription);

        return Result.Success();
    }


    public async Task<Result> PurchaseCourse(
        CourseId courseId,
        CustomerId customerId)
    {
        var course = await courseRepository.GetAsync(courseId);

        if (course is null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        var tutor = await tutorRepository
            .GetTutorByUserId(customerId);

        if (tutor is null)
        {
            return Result.Fail(CourseDomainError.NonExistTutorError);
        }

        var result = course.Purchase(tutor.Id);

        return result;
    }

    public async Task<Result> ReviewCourse(CourseId courseId, short rate, string detail, CustomerId customerId)
    {
        var courseFromDb = await courseRepository.GetAsync(courseId);

        if (courseFromDb == null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        if (courseFromDb.Status != Status.Confirmed || courseFromDb.TutorId is null)
        {
            return Result.Fail(CourseDomainError.NonExistCourseError);
        }

        if (courseFromDb.LearnerId != customerId)
        {
            return Result.Fail(CourseDomainError.IncorrectUserOfCourseError);
        }

        var result = courseFromDb.ReviewCourse(rate, detail);

        return result;
    }
}