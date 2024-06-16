using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

internal class SubjectRepository(AppDbContext appDbContext) : ISubjectRepository
{
    private static List<Subject> _subjects = new();

    public async Task<List<Subject>> GetListByIdsAsync(IEnumerable<SubjectId> subjectIds,
        CancellationToken cancellationToken = default)
    {
        if (_subjects.Count == 0)
        {
            _subjects = await appDbContext.Subjects.ToListAsync(cancellationToken);
        }

        return _subjects.Where(s => subjectIds.Contains(s.Id))
            .ToList();
    }

    public async Task<IList<Subject>> GetAllListAsync(CancellationToken cancellationToken = default)
    {
        if (_subjects.Count == 0)
            _subjects = await appDbContext.Subjects.ToListAsync(cancellationToken);

        return _subjects;
    }

    public async Task<Subject?> GetAsync(SubjectId subjectId, CancellationToken cancellationToken)
    {
        if (_subjects.Count == 0)
            _subjects = await appDbContext.Subjects.ToListAsync(cancellationToken);

        return _subjects.FirstOrDefault(s => s.Id == subjectId);
    }

    public IQueryable<Subject> GetAll() => appDbContext.Subjects;

    public Task InsertAsync(Subject subject)
    {
        appDbContext.Subjects.Add(subject);

        _subjects.Clear();

        return Task.CompletedTask;
    }
}