using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Admin.Application.Contracts.Courses.Dtos;

public class CourseForDetailDto
{
    //Basic Information
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public DateTime? LastModificationTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = Domain.Shared.Courses.Status.OnVerifying.ToString();
    public string LearningMode { get; set; } = Domain.Shared.Courses.LearningMode.Offline.ToString();

    public float SectionFee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    //Tutor related information
    public string GenderRequirement { get; set; } = Gender.None.ToString();
    public string AcademicLevelRequirement { get; set; } = AcademicLevel.Optional.ToString();

    //Student related information
    public string LearnerName { get; set; } = "";
    public string LearnerGender { get; set; } = "None";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;
    public Guid? LearnerId { get; set; }


    // Time related information
    public int SessionDuration { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    //Confirmed data related
    public Guid? TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhoneNumber { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;

    //List of Request

    public List<CourseRequestListForAdminDto> CourseRequestListForAdminDtos = new();
    public string? ReviewDetail { get; set; } = string.Empty;
    public int Rate { get; set; } = 0;
}

public class CourseRequestListForAdminDto : Entity<Guid>
{
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string AcademicLevel { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
    public Guid TutorId { get; set; }
}

public class CourseForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Course, Subject), CourseForDetailDto>()
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.LearnerName, src => src.Item1.LearnerName)
            .Map(des => des.Title, src => src.Item1.Title)
            .Map(des => des.Status, src => src.Item1.Status.ToString())
            .Map(des => des.GenderRequirement, src => src.Item1.GenderRequirement.ToString())
            .Map(des => des.LearnerGender, src => src.Item1.LearnerGender.ToString())
            .Map(des => des.AcademicLevelRequirement, src => src.Item1.AcademicLevelRequirement.ToString())
            .Map(des => des.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(des => des.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(des => des.SectionFee, src => src.Item1.SectionFee.Amount)
            .Map(des => des.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(des => des.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(des => des.SubjectName, src => src.Item2.Name)
            .Map(des => des.SubjectId, src => src.Item2.Id.Value)
            .Map(des => des.Address, src => src.Item1.Address)
            .Map(des => des.Description, src => src.Item1.Description)
            .Map(des => des.ContactNumber, src => src.Item1.ContactNumber)
            .Map(des => des.NumberOfLearner, src => src.Item1.NumberOfLearner)
            //.Map(des => des.CourseRequestListForAdminDtos, src => src.Item1.CourseRequests)
            .Map(des => des.ReviewDetail, src => src.Item1.Review != null ? src.Item1.Review.Detail : "")
            .Map(des => des.TutorId, src => src.Item1.TutorId != null ? src.Item1.TutorId.Value : Guid.Empty)
            .Map(des => des, src => src);

        config.NewConfig<Course, CourseForDetailDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des.CourseRequestListForAdminDtos, src => src.CourseRequests)
            .Map(des => des, src => src);

        config.NewConfig<(CourseRequest, Customer, Tutor), CourseRequestListForAdminDto>()
            .Map(des => des.Id, src => src.Item1.Id.Value)
            .Map(des => des.TutorId, src => src.Item3.Id.Value)
            .Map(des => des.RequestStatus, src => src.Item1.RequestStatus.ToString())
            .Map(des => des.TutorName, src => src.Item2.GetFullName())
            .Map(des => des.TutorPhone, src => src.Item2.PhoneNumber)
            .Map(des => des.TutorEmail, src => src.Item2.Email)
            .Map(des => des.University, src => src.Item3.University)
            .Map(des => des.AcademicLevel, src => src.Item3.AcademicLevel.ToString());
    }
}