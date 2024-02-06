using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Users.Tutors;
public class TutorForListDto : BasicAuditedEntityDto<Guid>
{
    //Admin information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = Domain.Shared.Courses.GenderEnum.Male.ToString();
    public int BirthYear { get; set; } = 1960;
    //public string WardId { get; set; } = "00001";
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    //Account References
    public string PhoneNumber { get; set; } = string.Empty;

    //is tutor related informtions
    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.Student.ToString();
    public string University { get; set; } = string.Empty;
    public short NumberOfRequests { get; set; } = 0;
    public bool IsVerified { get; set; } = false;
    public short Rate { get; set; } = 5;
    public short NumberOfChangeRequests  { get; set; } = 0;
}