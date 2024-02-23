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
}