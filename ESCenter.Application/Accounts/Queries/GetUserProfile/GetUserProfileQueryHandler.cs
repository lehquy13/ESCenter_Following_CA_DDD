using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    IUserRepository userRepository,
    ICurrentUserService currentUserService
)
    : QueryHandlerBase<GetUserProfileQuery, UserProfileDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<UserProfileDto>> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userProfileAsync =
            await userRepository.GetAsync(IdentityGuid.Create(currentUserService.UserId), cancellationToken);

        if (userProfileAsync is null)
        {
            return Result.Fail(UserError.NonExistUserError);
        }

        return Mapper.Map<UserProfileDto>(userProfileAsync);
    }
}