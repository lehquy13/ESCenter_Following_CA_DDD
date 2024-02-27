using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Subjects;
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

    public float SectionFee { get; set; } = 0;
    public float ChargeFee { get; set; } = 0;

    //Tutor related information
    public string GenderRequirement { get; set; } = "None";
    public string AcademicLevelRequirement { get; set; } = "Optional";

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

    public List<CourseRequestForDetailDto> CourseRequestForDetailDtos = new();
    public string? ReviewDetail { get; set; } = string.Empty;
    public int Rate { get; set; } = 0;
}

public class CourseForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tuple<Course, Subject>, CourseForDetailDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.Status, src => src.Item1.Status.ToString())
            .Map(dest => dest.GenderRequirement, src => src.Item1.GenderRequirement.ToString())
            .Map(dest => dest.LearnerGender, src => src.Item1.LearnerGender.ToString())
            .Map(dest => dest.AcademicLevelRequirement, src => src.Item1.AcademicLevelRequirement.ToString())
            .Map(dest => dest.LearningMode, src => src.Item1.LearningMode.ToString())
            .Map(dest => dest.ChargeFee, src => src.Item1.ChargeFee.Amount)
            .Map(dest => dest.SectionFee, src => src.Item1.SectionFee.Amount)
            .Map(dest => dest.SessionDuration, src => src.Item1.SessionDuration.Value)
            .Map(dest => dest.SessionPerWeek, src => src.Item1.SessionPerWeek.Value)
            .Map(dest => dest.SubjectName, src => src.Item2.Name)
            .Map(dest => dest.SubjectId, src => src.Item2.Id.Value)
            .Map(dest => dest.Address, src => src.Item1.Address)
            .Map(dest => dest.Description, src => src.Item1.Description)
            .Map(dest => dest.ContactNumber, src => src.Item1.ContactNumber)
            .Map(dest => dest.NumberOfLearner, src => src.Item1.NumberOfLearner)
            .Map(dest => dest.CourseRequestForDetailDtos, src => src.Item1.CourseRequests)
            .Map(dest => dest.ReviewDetail, src => src.Item1.Review != null ? src.Item1.Review.Detail : "")
            .Map(dest => dest.TutorId, src => src.Item1.TutorId != null ? src.Item1.TutorId.Value : Guid.Empty)
            .Map(dest => dest, src => src);

        config.NewConfig<Course, CourseForDetailDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CourseRequestForDetailDtos, src => src.CourseRequests)
            .Map(dest => dest, src => src);

        //     config.NewConfig<CourseRequest, CourseRequestForListDto>()
        //         .Map(dest => dest.Id, src => src.Id.Value)
        //         .Map(dest => dest.CourseRequestForDetailDtos, src => src.CourseRequests)
        //         .Map(dest => dest, src => src);
    }
}