using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Discoveries;

public interface IDiscoveryRepository : IRepository<Discovery, DiscoveryId>
{
    public IQueryable<DiscoverySubject> GetDiscoverySubjectAsQueryable();

    public Task<List<SubjectId>> GetUserDiscoverySubjects(CustomerId userGuid,
        CancellationToken cancellationToken);
}