using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.Aggregates.Subjects;

public interface ISubjectRepository : IRepository<Subject, SubjectId>
{
    Task<List<Subject>> GetListByIdsAsync(IEnumerable<SubjectId> subjectIds, CancellationToken cancellationToken = default);
}

