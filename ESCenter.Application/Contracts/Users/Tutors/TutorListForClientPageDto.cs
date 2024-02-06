using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorListForClientPageDto : EntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int BirthYear { get; set; } = 1960;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.Student.ToString();
    public string University { get; set; } = string.Empty;
}