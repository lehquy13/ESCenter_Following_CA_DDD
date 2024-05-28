using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.EntityFrameworkCore;
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
        var result = AppDbContext.Courses
            .AsNoTracking()
            .Where(x => x.Status == Status.Confirmed)
            .Where(x => x.CreationTime > thisMonth.AddMonths(-1))
            .GroupBy(x => x.TutorId)
            .OrderByDescending(x => x.Count())
            .Select(x => x.Key);

        var tutors = await AppDbContext.Tutors
            .Where(x => result.Contains(x.Id))
            .Take(10)
            .ToListAsync();

        return tutors;
    }

    public Task<Tutor?> GetTutorByUserId(CustomerId userId, CancellationToken cancellationToken = default)
    {
        return AppDbContext.Tutors
            .FirstOrDefaultAsync(x =>
                    x.CustomerId == userId,
                cancellationToken: cancellationToken);
    }

    public Task RemoveChangeVerification(ChangeVerificationRequestId id)
    {
        var changeVerificationRequest = AppDbContext.ChangeVerificationRequests
            .FirstOrDefault(x => x.Id == id);

        if (changeVerificationRequest is not null)
        {
            AppDbContext.ChangeVerificationRequests.Remove(changeVerificationRequest);
        }

        return Task.CompletedTask;
    }

    public async Task<List<TutorId>> GetTutorsBySubjectId(SubjectId subjectId, CancellationToken cancellationToken)
    {
        return await AppDbContext.Tutors
            .Where(x => x.TutorMajors.Any(xx => xx.SubjectId == subjectId))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}