using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class DiscoveryRepository(AppDbContext appDbContext, IAppLogger<RepositoryImpl<Discovery, DiscoveryId>> logger)
    : RepositoryImpl<Discovery, DiscoveryId>(appDbContext, logger), IDiscoveryRepository
{
    public IQueryable<DiscoverySubject> GetDiscoverySubjectAsQueryable()
    {
        return AppDbContext.Set<DiscoverySubject>();
    }

    public async Task<List<SubjectId>> GetUserDiscoverySubjects(IdentityGuid userGuid, 
        CancellationToken cancellationToken)
    {
        return await AppDbContext.Database.SqlQuery<SubjectId>(
            $"""
              SELECT SubjectId 
              FROM DiscoverySubjects 
              WHERE UserId = {userGuid.Value}
              JOIN DISCOVERYUSERS ON DISCOVERYUSERS.DiscoveryId = DISCOVERYSUBJECTS.DiscoveryId 
              """).ToListAsync(cancellationToken);
    }

    public IQueryable<DiscoveryUser> GetDiscoveryUserAsQueryable()
    {
        return AppDbContext.Set<DiscoveryUser>();
    }
}