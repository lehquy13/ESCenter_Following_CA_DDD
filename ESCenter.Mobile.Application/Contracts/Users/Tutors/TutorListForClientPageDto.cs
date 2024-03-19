using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Mobile.Application.Contracts.Users.Tutors;

public class TutorListForClientPageDto : EntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int BirthYear { get; set; } = 1960;
    public string Description { get; set; } = string.Empty;
    public string Avatar { get; set; } = @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public string AcademicLevel { get; set; } = Domain.Shared.Courses.AcademicLevel.UnderGraduated.ToString();
    public string University { get; set; } = string.Empty;
}

public class TutorListForClientPageDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(User, Tutor), TutorListForClientPageDto>()
            .Map(dest => dest.Id, src => src.Item2.Id.Value)
            .Map(des => des, src => src.Item2)
            .Map(des => des, src => src.Item1);
    }
}