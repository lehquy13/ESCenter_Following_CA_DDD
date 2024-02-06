using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Courses.Dtos;

public sealed class CourseForListDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = "Offline";

    public float Fee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";

    public string LearnerGender { get; set; } = "None";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;

    public int MinutePerSession { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    public string Address { get; set; } = string.Empty;

    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}