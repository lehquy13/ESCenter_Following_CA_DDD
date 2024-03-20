using System.Collections;
using ESCenter.Application.Contracts.Commons;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using Tutor = ESCenter.Domain.Aggregates.Tutors.Tutor;

namespace ESCenter.Client.Application.Contracts.Users.Tutors;

public class TutorDetailForClientDto : BasicAuditedEntityDto<Guid>
{
    public string FullName { get; init; } = string.Empty;
    public string Gender { get; init; } = "Male";
    public int BirthYear { get; init; } = 1960;
    public string Address { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public string Avatar { get; init; } =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public string AcademicLevel { get; init; } = "Student";
    public string University { get; init; } = string.Empty;
    public short Rate { get; init; } = 5;
    public List<string> TutorMajors { get; init; } = new();
    public List<ReviewDto> Reviews { get; set; } = null!;
}

public class ReviewDto : BasicAuditedEntityDto<Guid>
{
    public string LearnerName { get; init; } = "";
    public short Rate { get; init; } = 5;
    public string Detail { get; init; } = "";
}

public class TutorDetailForClientDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Tutor, Customer), TutorDetailForClientDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.FullName, src => src.Item2.GetFullName())
            .Map(dest => dest.AcademicLevel, src => src.Item1.AcademicLevel.ToString())
            .Map(dest => dest.Rate, src => src.Item1.Rate)
            .Map(dest => dest.University, src => src.Item1.University)
            .Map(dest => dest.Address, src => src.Item2.Address.City + src.Item2.Address.Country)
            .Map(dest => dest.TutorMajors, src => src.Item1.TutorMajors.Select(x => x.SubjectName))
            .Map(dest => dest, src => src.Item2)
            .Map(dest => dest, src => src);

        config.NewConfig<(Review, string), ReviewDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.Id, src => src.Item2)
            .Map(dest => dest, src => src);
    }
}