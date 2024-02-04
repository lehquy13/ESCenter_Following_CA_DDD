using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Subjects;

public class Subject : FullAuditedAggregateRoot<SubjectId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    
    private Subject()
    {
    }
    
    public static Subject Create(string name, string description)
    {
        return new Subject()
        {
            Name = name,
            Description = description
        };
    }

    public void SetName(string subjectDtoName)
    {
        // Add more validation here
        Name = subjectDtoName;
    }

    public void SetDescription(string subjectDtoDescription)
    {
        // Add more validation here
        Description = subjectDtoDescription;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}

