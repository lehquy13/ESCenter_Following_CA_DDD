using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Application.Mapping;

public class TutorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Tutor, TutorForProfileDto>()
            .Map(dest => dest.ChangeVerificationRequestDtos, src => src.ChangeVerificationRequests)
            .Map(dest => dest.TutorVerificationInfoDtos, src => src.TutorVerificationInfos)
            .Map(dest => dest, src => src);

        config.NewConfig<(User, Tutor), TutorForDetailDto>()
            .Map(des => des, src => src.Item2)
            .Map(des => des, src => src.Item1);

        config.NewConfig<(User, Tutor), TutorListForClientPageDto>()
            .Map(des => des, src => src.Item2)
            .Map(des => des, src => src.Item1);

        config.NewConfig<(Tutor, User, string ), TutorRequestForListDto>()
            .Map(dest => dest.Tutor, src => src.Item1)
            .Map(dest => dest.RequestMessage, src => src.Item3)
            .Map(dest => dest, src => src.Item2);

    }
}