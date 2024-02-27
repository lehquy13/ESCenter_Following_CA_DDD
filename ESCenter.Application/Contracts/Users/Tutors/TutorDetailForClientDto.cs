using ESCenter.Application.Contracts.Commons;
using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using Tutor = ESCenter.Domain.Aggregates.Tutors.Tutor;

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

public class TutorDetailForClientDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Tutor, User, IEnumerable<Review>), TutorDetailForClientDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.FullName, src => src.Item2.GetFullName())
            .Map(dest => dest.AcademicLevel, src => src.Item1.AcademicLevel.ToString())
            .Map(dest => dest.Rate, src => src.Item1.Rate)
            .Map(dest => dest.TutorMajors, src => src.Item1.TutorMajors.Select(x => x.SubjectName))
            .Map(dest => dest.Reviews, src => src.Item3)
            .Map(dest => dest, src => src);
    }
}