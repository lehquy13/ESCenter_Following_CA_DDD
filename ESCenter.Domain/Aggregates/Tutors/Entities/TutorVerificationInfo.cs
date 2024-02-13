using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class TutorVerificationInfo : Entity<int>
{
    private TutorVerificationInfo()
    {
    }

    public static TutorVerificationInfo Create(string image, TutorId tutorId)
    {
        return new TutorVerificationInfo()
        {
            TutorId = tutorId,
            Image = image
        };
    }

    public TutorId TutorId { get; private set; } = null!;
    public string Image { get; private set; } = "doc_contract.png";
}