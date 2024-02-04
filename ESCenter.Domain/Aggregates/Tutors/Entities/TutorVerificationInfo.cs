using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class TutorVerificationInfo : Entity<int>
{
    private TutorVerificationInfo(string image, IdentityGuid tutorId)
    {
        Image = image;
        TutorId = tutorId;
    }
    
    public static TutorVerificationInfo Create(string image, IdentityGuid tutorId)
    {
        return new TutorVerificationInfo(image, tutorId);
    }
    
    public IdentityGuid TutorId { get; private set; } = null!;
    public string Image { get; private set; } = "doc_contract.png";
}