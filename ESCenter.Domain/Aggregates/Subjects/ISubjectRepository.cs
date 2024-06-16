using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Subjects;

public interface ISubjectRepository : IRepository
{
    Task<List<Subject>> GetListByIdsAsync(IEnumerable<SubjectId> subjectIds,
        CancellationToken cancellationToken = default);

    Task<IList<Subject>> GetAllListAsync(CancellationToken cancellationToken = default);
    Task<Subject?> GetAsync(SubjectId subjectId, CancellationToken cancellationToken);
    IQueryable<Subject> GetAll();
    Task InsertAsync(Subject subject);
}