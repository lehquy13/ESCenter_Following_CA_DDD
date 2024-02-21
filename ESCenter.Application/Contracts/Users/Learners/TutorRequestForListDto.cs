using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Application.Contracts.Users.Learners;

public class TutorRequestForListDto
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public TutorForListDto Tutor { get; set; } = null!;
    
    public Guid LearnerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; }= string.Empty;
    public string RequestMessage { get; set; } = null!;
}

public class TutorRequestForListDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Tutor, User, string ), TutorRequestForListDto>()
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest.Tutor, src => src.Item1)
            .Map(dest => dest.RequestMessage, src => src.Item3)
            .Map(dest => dest, src => src.Item2);
    }
}