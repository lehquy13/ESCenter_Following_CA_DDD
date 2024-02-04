using ESCenter.Application.Contracts.Commons.Primitives;
using ESCenter.Application.Contracts.Commons.Primitives.Auditings;
using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Application.Contracts.Users;

public class AdminDto : FullAuditedAggregateRootDto<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = GenderEnum.Male;
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Admin;
}