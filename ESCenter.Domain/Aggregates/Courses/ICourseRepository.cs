using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Courses;

public interface ICourseRepository : IRepository<Course, CourseId>
{
    Task<List<Course>> GetLearningCoursesByUserId(IdentityGuid learnerId);
    Task<bool> IsCoursesRequestedByTutor(IdentityGuid tutorId, CourseId classId);
    Task<Course?> GetCourseByCourseRequestId(CourseRequestId courseRequestId, CancellationToken cancellationToken);
}