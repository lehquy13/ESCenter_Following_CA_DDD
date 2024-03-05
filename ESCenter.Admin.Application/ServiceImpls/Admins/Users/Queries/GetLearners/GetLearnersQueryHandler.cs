using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Users.Queries.GetLearners;

public class GetLearnersQueryHandler(
    IUserRepository userRepository,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetLearnersQuery, List<UserForListDto>>(unitOfWork, logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<List<UserForListDto>>> Handle(GetLearnersQuery request,
        CancellationToken cancellationToken)
    {
        var learnersFromDb = await userRepository.GetLearners();
        var userForListDtos = _mapper.Map<List<UserForListDto>>(learnersFromDb);
        
        return userForListDtos;
    }
}