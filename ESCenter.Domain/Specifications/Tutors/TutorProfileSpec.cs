using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Tutors;

public class TutorProfileSpec : FindSpecificationBase<Tutor, CustomerId>
{
    public TutorProfileSpec(CustomerId tutorId) : base(tutorId)
    {
        IncludeStrings.Add(nameof(Tutor.Verifications));
        IncludeStrings.Add(nameof(Tutor.ChangeVerificationRequest));
    }
}