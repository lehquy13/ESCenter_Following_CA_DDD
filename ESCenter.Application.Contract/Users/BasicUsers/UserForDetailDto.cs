using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Users.BasicUsers;
public class UserForDetailDto : BasicAuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = Domain.Shared.Courses.GenderEnum.Male;
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = Domain.Shared.Courses.UserRole.Learner.ToString();

}

