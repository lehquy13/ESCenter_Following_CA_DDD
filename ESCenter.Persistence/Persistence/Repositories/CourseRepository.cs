using ESCenter.Domain.Aggregates.CourseRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class CourseRepository(
    AppDbContext appDbContext,
    IAppLogger<CourseRepository> appLogger)
    : RepositoryImpl<Course, CourseId>(appDbContext, appLogger), ICourseRepository
{
    public Task<List<Course>> GetLearningCoursesByUserId(IdentityGuid learnerId)
    {
        return AppDbContext.Courses
            .Where(x => x.LearnerId == learnerId)
            .ToListAsync();
    }

    public async Task<bool> IsCoursesRequestedByTutor(IdentityGuid tutorId, CourseId classId)
    {
        return await AppDbContext.CourseRequests
            .AnyAsync(x => x.TutorId == tutorId && x.CourseId == classId);
    }

    public Task<Course?> GetCourseByCourseRequestId(CourseRequestId courseRequestId, CancellationToken cancellationToken)
    {
        return AppDbContext.Courses
            .Include(x => x.CourseRequests)
            .FirstOrDefaultAsync(x => x.CourseRequests.Any(rq => rq.Id == courseRequestId), cancellationToken);
    }
}