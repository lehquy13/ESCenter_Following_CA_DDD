using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Mapster;

namespace ESCenter.Application.Contracts.Courses.Dtos;

public class CourseForDetailDto
{
    //Basic Information
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public DateTime? LastModificationTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "OnVerifying";
    public string LearningMode { get; set; } = "Offline";

    public float Fee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    //Tutor related information
    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";

    //Student related information
    public string LearnerName { get; set; } = "";
    public string LearnerGender { get; set; } = "None";
    public int NumberOfLearner { get; set; } = 1;
    public string ContactNumber { get; set; } = string.Empty;
    public int? LearnerId { get; set; }


    // Time related information
    public int MinutePerSession { get; set; } = 90;
    public int SessionPerWeek { get; set; } = 2;

    // Address related information
    public string Address { get; set; } = string.Empty;

    //Subject related information
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    //Confirmed data related
    public int? TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhoneNumber { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;

    //List of Request

    public List<CourseRequestForDetailDto> CourseRequestForDetailDtos = new();
    public string? ReviewDetail { get; set; } = string.Empty;
    public int Rate { get; set; } = 0;
}

public class CourseForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Course, Subject, User, IdentityUser), CourseForDetailDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.SubjectName, src => src.Item2.Name)
            .Map(dest => dest.TutorName, src => src.Item3.GetFullName())
            .Map(dest => dest.TutorPhoneNumber, src => src.Item4.PhoneNumber)
            .Map(dest => dest.TutorEmail, src => src.Item4.Email)
            .Map(dest => dest, src => src);
        
        config.NewConfig<Course, CourseForDetailDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CourseRequestForDetailDtos, src => src.CourseRequests)
            .Map(dest => dest, src => src);
    }
}