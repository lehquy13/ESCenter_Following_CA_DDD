using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Tutors;

public class GetTutorByCustomerIdSpec : SpecificationBase<Tutor>
{
    public GetTutorByCustomerIdSpec(CustomerId customerId)
    {
        Criteria = tutor => tutor.CustomerId == customerId;
    }
}