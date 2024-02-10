using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearners;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutors;

public class GetAllTutorQueryHandler(
    IUserRepository userRepository,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetAllTutorsQuery, IEnumerable<UserForListDto>>(unitOfWork, logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<IEnumerable<UserForListDto>>> Handle(GetAllTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutors = await userRepository.GetTutors();
        var userForListDtos = _mapper.Map<List<UserForListDto>>(tutors);

        return userForListDtos;
    }
}