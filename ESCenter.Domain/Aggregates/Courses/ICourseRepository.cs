using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Courses;

public interface ICourseRepository : IRepository<Course, CourseId>
{
    Task<List<Course>> GetLearningCoursesByUserId(CustomerId learnerId);
    Task<bool> IsCoursesRequestedByTutor(CustomerId tutorId, CourseId classId);
    Task<Course?> GetCourseByCourseRequestId(CourseRequestId courseRequestId, CancellationToken cancellationToken);
    Task<List<Course>> GetAllTutorRelatedCourses(TutorId tutorId);
}