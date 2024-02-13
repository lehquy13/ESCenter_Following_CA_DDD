using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries.Entities;

public class DiscoverySubject : Entity<int>
{
    public DiscoveryId DiscoveryId { get; private set; } = null!;
    public SubjectId SubjectId { get; private set; } = null!;
    public string SubjectName { get; private set; } = null!;

    private DiscoverySubject()
    {
    }

    public static DiscoverySubject Create(DiscoveryId discoveryId, SubjectId subjectId, string subjectName)
    {
        return new DiscoverySubject
        {
            DiscoveryId = discoveryId,
            SubjectId = subjectId,
            SubjectName = subjectName
        };
    }
}