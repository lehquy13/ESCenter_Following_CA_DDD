using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class TutorRepository(
    AppDbContext appDbContext,
    IAppLogger<TutorRepository> appLogger)
    : RepositoryImpl<Tutor, IdentityGuid>(appDbContext, appLogger), ITutorRepository
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
}