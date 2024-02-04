using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Persistence.Entity_Framework_Core;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class DiscoveryRepository(AppDbContext appDbContext, IAppLogger<RepositoryImpl<Discovery, DiscoveryId>> logger)
    : RepositoryImpl<Discovery, DiscoveryId>(appDbContext, logger), IDiscoveryRepository
{
    public IQueryable<DiscoverySubject> GetDiscoverySubjectAsQueryable()
    {
        return AppDbContext.Set<DiscoverySubject>();
    }

    public IQueryable<DiscoveryUser> GetDiscoveryUserAsQueryable()
    {
        return AppDbContext.Set<DiscoveryUser>();
    }
}