using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Tutors;

public class TutorByCustomerIdSpec : SpecificationBase<Tutor>
{
    public TutorByCustomerIdSpec(CustomerId customerId)
    {
        Criteria = tutor => tutor.CustomerId == customerId;
    }
}