using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using ESCenter.Admin.Application.ServiceImpls.Customers.Queries.GetLearners;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutors;

public class GetAllTutorsBySubjectIdQueryHandler(
    ICustomerRepository customerRepository,
    ITutorRepository tutorRepository,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllTutorsBySubjectIdQuery, List<UserForListDto>>(logger, mapper)
{
    public override async Task<Result<List<UserForListDto>>> Handle(GetAllTutorsBySubjectIdQuery request,
        CancellationToken cancellationToken)
    {
        var tutors = await tutorRepository.GetTutorsBySubjectId(SubjectId.Create(request.SubjectId),cancellationToken);

        var tutorInfos = await customerRepository.GetTutorsByTutorIds(tutors);
        
        var userForListDtos = Mapper.Map<List<UserForListDto>>(tutorInfos);

        return userForListDtos;
    }
}