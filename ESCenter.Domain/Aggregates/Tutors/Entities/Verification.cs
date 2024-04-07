using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class Verification : Entity<VerificationId> // TODO: Update the verification to be an audited entity
{
    public TutorId TutorId { get; private set; } = null!;
    public string Image { get; private set; } = null!;
    
    private Verification()
    {
    }

    public static Verification Create(string image, TutorId tutorId)
    {
        return new Verification()
        {
            Id = VerificationId.Create(),
            TutorId = tutorId,
            Image = image
        };
    }
}