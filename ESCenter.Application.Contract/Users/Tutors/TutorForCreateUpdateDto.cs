using ESCenter.Application.Contracts.Commons.Primitives;
using ESCenter.Application.Contracts.Commons.Primitives.Auditings;
using ESCenter.Application.Contracts.Users.BasicUsers;

namespace ESCenter.Application.Contracts.Users.Tutors;
public class TutorForCreateUpdateDto : BasicAuditedEntityDto<int>
{
    //Admin information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    //public string WardId { get; set; } = "00001";
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    //Account References
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;

    public string PhoneNumber { get; set; } = string.Empty;
    //public string Password { get; set; } = "1q2w3E*";

    //is tutor related informtions
    public string Role { get; set; } = "Tutor";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = "Empty";
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<int> Majors { get; set; } = new();

}

public class TutorCreateUpdateDto : EntityDto<Guid>
{
    public UserForCreateDto UserForCreateDto { get; set; } = null!;
    public TutorProfileCreateDto TutorForCreateUpdateDto { get; set; } = null!;
}

public record TutorProfileCreateDto(string AcademicLevel, string University, List<int> Majors);

public class TutorForRegistrationDto : EntityDto<Guid>
{
    public string Description { get; set; } = string.Empty;

    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public List<string> Majors { get; set; } = new();
    public List<TutorVerificationInfoDto> TutorVerificationInfoDtos { get; set; } = new();

}