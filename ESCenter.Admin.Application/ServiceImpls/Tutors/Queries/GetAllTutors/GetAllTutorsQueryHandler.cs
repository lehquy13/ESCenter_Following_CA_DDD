using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using ESCenter.Admin.Application.ServiceImpls.Users.Queries.GetLearners;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutors;

public class GetAllTutorsQueryHandler(
    IUserRepository userRepository,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetAllTutorsQuery, List<UserForListDto>>(unitOfWork, logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<List<UserForListDto>>> Handle(GetAllTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutors = await userRepository.GetTutors();
        var userForListDtos = _mapper.Map<List<UserForListDto>>(tutors);

        return userForListDtos;
    }
}