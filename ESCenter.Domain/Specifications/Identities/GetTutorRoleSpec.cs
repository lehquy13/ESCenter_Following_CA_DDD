using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Identities;

public class GetTutorRoleSpec : SpecificationBase<IdentityRole>
{
    public GetTutorRoleSpec()
    {
        Criteria = role => role.Name == UserRole.Tutor.ToString();
    }
}
