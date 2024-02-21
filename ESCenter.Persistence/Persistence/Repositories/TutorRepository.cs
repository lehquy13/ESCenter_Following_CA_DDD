using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class TutorRepository(
    AppDbContext appDbContext,
    IAppLogger<TutorRepository> appLogger)
    : RepositoryImpl<Tutor, TutorId>(appDbContext, appLogger), ITutorRepository
{
    public async Task<List<Tutor>> GetPopularTutors()
    {
        var thisMonth = new DateTime(
            DateTime.Now.Year,
            DateTime.Now.Month,
            1
        );
        var result = AppDbContext.CourseRequests
            .AsNoTracking()
            .Where(x => x.RequestStatus == RequestStatus.Success)
            .Where(x => x.CreationTime > thisMonth)
            .GroupBy(x => x.TutorId)
            .OrderByDescending(x => x.Count())
            .Select(x => x.Key);

        var tutors = await AppDbContext.Tutors
            .Where(x => result.Contains(x.Id))
            .ToListAsync();

        return tutors;
    }

    public Task<Tutor?> GetTutorByUserId(IdentityGuid userId)
    {
        return AppDbContext.Tutors
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<TutorMajor>> GetTutorMajors(TutorId tutorId, CancellationToken cancellationToken)
    {
        // var subjects = await AppDbContext.Subjects
        //     .AsNoTracking()
        //     .GroupJoin(
        //         AppDbContext.TutorMajors.Where(x => x.TutorId == tutorId),
        //         subject => subject.Id,
        //         tutorMajor => tutorMajor.SubjectId,
        //         (subject, tutorMajors) => new { Subject = subject, TutorMajors = tutorMajors.DefaultIfEmpty() }
        //     )
        //     .SelectMany(
        //         x => x.TutorMajors,
        //         (subject, tutorMajor) => subject
        //     )
        //     .ToListAsync(cancellationToken);

        
        // var subjects = await AppDbContext.Subjects
        //     .AsNoTracking()
        //     .LeftJoin(AppDbContext.TutorMajors.Where(x => x.TutorId == tutorId),
        //         subject => subject.Id,
        //         tutorMajor => tutorMajor.SubjectId,
        //         (subject, tutorMajor) => subject)
        //     .ToListAsync(cancellationToken);

        return await AppDbContext.TutorMajors
            .AsNoTracking()
            .Where(x => x.TutorId == tutorId)
            .ToListAsync(cancellationToken);
    }
}