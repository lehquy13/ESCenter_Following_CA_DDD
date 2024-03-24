using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class SubjectRepository(
    AppDbContext appDbContext,
    IAppLogger<SubjectRepository> appLogger)
    : RepositoryImpl<Subject, SubjectId>(appDbContext, appLogger), ISubjectRepository
{
    public Task<List<Subject>> GetListByIdsAsync(IEnumerable<SubjectId> subjectIds, CancellationToken cancellationToken = default)
    {
        return AppDbContext.Subjects
            .Where(s => subjectIds.Contains(s.Id))
            .ToListAsync(cancellationToken);
    }
}