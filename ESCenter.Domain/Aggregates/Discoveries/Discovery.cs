using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Discoveries;

public class Discovery : AggregateRoot<DiscoveryId>
{
    private List<DiscoverySubject> _discoverySubjects = [];
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private static List<Discovery> _discoveries = [];

    public static IEnumerable<Discovery> Discoveries()
    {
        if (_discoveries.Count == 0)
        {
            _discoveries = SeedDiscoveries();
        }

        return _discoveries;
    }

    public IReadOnlyCollection<DiscoverySubject> DiscoverySubjects => _discoverySubjects.AsReadOnly();

    private Discovery()
    {
    }

    private static Discovery Create(string title, string description, List<DiscoverySubject> discoverySubjects)
        => new()
        {
            Title = title,
            Description = description,
            _discoverySubjects = discoverySubjects
        };

    private static List<Discovery> SeedDiscoveries()
    {
        var discovery1 = Create("Information Technology",
            "The study of information and computational systems",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(1), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(2), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(3), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(4), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(5), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(6), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(7), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(8), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(12), ""),
            ]);
        var discovery2 = Create("Programming",
            "Basic principles and concepts of programming",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(2), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(3), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(4), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(5), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(6), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(7), ""),
            ]);

        var discovery3 = Create("Language",
            "Study of language and culture",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(10), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(11), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(12), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(13), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(14), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(15), ""),
            ]);

        var discovery4 = Create("Art",
            "Study of art and culture",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(16), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(17), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(18), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(19), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(20), ""),
            ]);

        var discovery5 = Create("Fitness and Health",
            "Study of fitness and health",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(21), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(22), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(23), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(24), ""),
            ]);

        var discovery6 = Create("Science",
            "Study of science",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(24), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(25), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(26), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(27), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(29), ""),
            ]);

        var discovery7 = Create("Social",
            "Study of social",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(28), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(29), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(30), ""),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(31), ""),
            ]);

        return new List<Discovery>
        {
            discovery1,
            discovery2,
            discovery3,
            discovery4,
            discovery5,
            discovery6,
            discovery7
        };
    }
}