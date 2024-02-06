using ESCenter.Application.Contract.Commons;
using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Application.Contract.Courses.Dtos;

public class CourseDto : BasicAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.OnVerifying;
    public LearningMode LearningMode { get; set; } = LearningMode.Offline;

    public float Fee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    //Tutor related information

    public string GenderEnumRequirement { get; set; } = GenderEnum.None;
    public AcademicLevel AcademicLevelRequirement { get; set; } = AcademicLevel.Optional;

    //Learner related information
    public string LearnerName { get; set; } = string.Empty;
    public string LearnerGenderEnum { get; set; } = GenderEnum.Male;
    public short NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;
    public Guid? LearnerId { get; set; }
   
    // Time related information
    public short MinutePerSession { get; set; } = 90;
    public short SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public short SubjectId { get; set; }
    public SubjectDto SubjectDto { get; set; }= null!;

    //Request of class
    public List<CourseRequestDto> CourseRequestDtos { get; set; } = new();
    public ReviewDetailDto ReviewDetailDtos { get; set; } = new();
}


