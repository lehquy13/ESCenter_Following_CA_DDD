using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Tutors;

public class TutorProfileSpec : FindSpecificationBase<Tutor, IdentityGuid>
{
    public TutorProfileSpec(IdentityGuid tutorId) : base(tutorId)
    {
        IncludeStrings.Add(nameof(Tutor.TutorVerificationInfos));
        IncludeStrings.Add(nameof(Tutor.ChangeVerificationRequests));
    }
}