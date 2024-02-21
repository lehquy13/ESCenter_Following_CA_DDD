using ESCenter.Domain.Aggregates.Tutors.Entities;
using Mapster;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class ChangeVerificationRequestDto 
{
    public int Id { get; set; }
    public int TutorId { get; set; }
    public string RequestStatus { get; set; } = null!;
    public List<string> ChangeVerificationRequestDetails { get; set; } = null!;
}

public class ChangeVerificationRequestDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ChangeVerificationRequest, ChangeVerificationRequestDto>()
            .Map(dest => dest.ChangeVerificationRequestDetails,
                src => src.ChangeVerificationRequestDetails.Select(x => x.ImageUrl).ToList())
            .Map(dest => dest, src => src);
    }
}