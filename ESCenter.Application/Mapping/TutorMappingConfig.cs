using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Clients.Tutors.Queries;
using ESCenter.Domain.Aggregates.Courses;
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

        config.NewConfig<(Tutor, User, IEnumerable<Review>), TutorDetailForClientDto>()
            .Map(dest => dest.FullName, src => src.Item2.GetFullName())
            .Map(dest => dest.AcademicLevel, src => src.Item1.AcademicLevel.ToString())
            .Map(dest => dest.Rate, src => src.Item1.Rate)
            .Map(dest => dest.TutorMajors, src => src.Item1.TutorMajors.Select(x => x.SubjectName))
            .Map(dest => dest.Reviews, src => src.Item3)
            .Map(dest => dest, src => src);
    }
}