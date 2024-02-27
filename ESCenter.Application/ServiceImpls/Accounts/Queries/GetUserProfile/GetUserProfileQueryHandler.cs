using ESCenter.Application.Contracts.Profiles;
using ESCenter.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetUserProfile;

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
        try
        {
            // This query includes verification information, major information, requests getting class
            if (currentUserService.CurrentUserId is null)
            {
                return Result.Fail(AccountServiceError.UnauthorizedError);
            }

            var userProfileAsync =
                await userRepository.GetAsync(
                    IdentityGuid.Create(new Guid(currentUserService.CurrentUserId)), cancellationToken);
            
            if (userProfileAsync is null)
            {
                return Result.Fail(UserError.NonExistUserError);
            }

            return Mapper.Map<UserProfileDto>(userProfileAsync);
        }
        catch (Exception ex)
        {
            return Result.Fail($"AccountServiceError.FailToGetTutorProfileWithException {ex.Message}");
        }
    }
}