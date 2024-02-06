using ESCenter.Application.Contracts.Users.BasicUsers;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearnerDetail;

public class GetLearnerDetailQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<GetLearnerDetailQueryHandler> logger,
    IMapper mapper,
    IIdentityRepository identityRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor)
    : QueryHandlerBase<GetLearnerDetail, UserForDetailDto>(unitOfWork, logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<UserForDetailDto>> Handle(GetLearnerDetail request,
        CancellationToken cancellationToken)
    {
        var identityId = IdentityGuid.Create(request.Id);
        var learnerFromDb =
            from identityUser in identityRepository.GetAll()
            join user in userRepository.GetAll() on identityUser.Id equals user.Id
            where identityUser.Id == identityId
            select new
            {
                Identity = identityUser,
                User = user
            };

        var resultFromDb = await asyncQueryableExecutor.FirstOrDefaultAsync(learnerFromDb, false,
            cancellationToken: cancellationToken);

        if (resultFromDb is null)
        {
            return Result.Fail(UserAppServiceError.NonExistUserError);
        }

        // TODO: this mapper may cause some problems
        var userForDetailDto = _mapper.Map<UserForDetailDto>(resultFromDb);
        return userForDetailDto;
    }
}