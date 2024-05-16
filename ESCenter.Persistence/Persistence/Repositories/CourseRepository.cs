using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class CourseRepository(
    AppDbContext appDbContext,
    IAppLogger<CourseRepository> appLogger)
    : RepositoryImpl<Course, CourseId>(appDbContext, appLogger), ICourseRepository
{
    public Task<List<Course>> GetLearningCoursesByUserId(CustomerId learnerId)
    {
        return AppDbContext.Courses
            .Where(x => x.LearnerId == learnerId)
            .ToListAsync();
    }

    public async Task<bool> IsCoursesRequestedByTutor(CustomerId tutorId, CourseId classId)
    {
        return await AppDbContext.CourseRequests
            .AnyAsync(x => x.TutorId == tutorId && x.CourseId == classId);
    }

    public Task<Course?> GetCourseByCourseRequestId(CourseRequestId courseRequestId,
        CancellationToken cancellationToken)
    {
        return AppDbContext.Courses
            .Include(x => x.CourseRequests)
            .FirstOrDefaultAsync(x =>
                x.CourseRequests.Any(rq => rq.Id == courseRequestId), cancellationToken);
    }

    public Task<List<Course>> GetAllTutorRelatedCourses(TutorId tutorId)
    {
        // Get all courses that are requested by tutor or owned by tutor
        return AppDbContext.Courses
            .Include(x => x.CourseRequests)
            .Where(x => x.CourseRequests.Any(rq => rq.TutorId == tutorId) || x.TutorId == tutorId)
            .ToListAsync();
    }
}