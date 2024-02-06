using ESCenter.Application.Contract.Users.BasicUsers;
using ESCenter.Application.Contract.Users.Tutors;
using ESCenter.Application.ServiceImpls.Clients.Tutors.Queries;
using ESCenter.Domain.Aggregates.Users.Identities;
using Mapster;

namespace ESCenter.Administrator.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<TutorForProfileDto, UserForDetailDto>();
        // config.NewConfig<int DeleteSubjectCommand>()
        //     .Map(dest => dest.SubjectId, src => src);
        //
        // config.NewConfig<CreateUpdateSubjectDto, CreateUpdateSubjectCommand>()
        //     .Map(dest => dest.SubjectDto, src => src);


        //----------------------------------------------------------------


    }
}


