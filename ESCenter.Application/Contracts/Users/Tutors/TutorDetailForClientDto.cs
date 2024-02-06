using ESCenter.Application.Contracts.Commons;
using ESCenter.Application.Contracts.Courses.Dtos;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorDetailForClientDto : BasicAuditedEntityDto<Guid>
{
    //Admin information
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public int BirthYear { get; set; } = 1960;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Image { get; set; } =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
    public string AcademicLevel { get; set; } = "Student";
    public string University { get; set; } = string.Empty;
    public short Rate { get; set; } = 5;
    public List<string> TutorMajors { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = null!;
}