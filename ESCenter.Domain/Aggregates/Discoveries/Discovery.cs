using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries;

public class Discovery : AggregateRoot<DiscoveryId>
{
    private List<DiscoverySubject> _discoverySubjects = new();
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public IEnumerable<DiscoverySubject> DiscoverySubjects => _discoverySubjects.AsReadOnly();
    
    private Discovery()
    {
    }

    public static Discovery Create(string title, string description, List<DiscoverySubject> discoverySubjects)
    {
        return new Discovery
        {
            Title = title,
            Description = description,
            _discoverySubjects = discoverySubjects
        };
    }
}