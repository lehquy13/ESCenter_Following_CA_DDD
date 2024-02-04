using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class TutorMajor : Entity<TutorMajorId>
{
    public IdentityGuid TutorId { get; private set; } = null!;
    public SubjectId SubjectId { get; private set; } = null!;
    public string SubjectName { get; private set; } = null!;
    private TutorMajor()
    {
    }

    public static TutorMajor Create(IdentityGuid tutorId, SubjectId subjectId, string subjectName)
    {
        return new()
        {
            TutorId = tutorId,
            SubjectId = subjectId,
            SubjectName = subjectName
        };
    }
}