using ESCenter.Application.Contracts.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contracts.Users.BasicUsers;
public class UserForListDto : BasicAuditedEntityDto<int>
{
    //User information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    //Account References
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string Role { get; set; } = "Learner";

}

