using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.Login;

public class LoginQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<LoginQueryHandler> logger,
    IMapper mapper,
    IIdentityRepository identityRepository,
    IReadOnlyRepository<IdentityRole, IdentityRoleId> identityRoleRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IJwtTokenGenerator jwtTokenGenerator
)
    : QueryHandlerBase<LoginQuery, AuthenticationResult>(unitOfWork, logger, mapper)
{
    public override async Task<Result<AuthenticationResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var identityUserQ =
                from identity in identityRepository.GetAll()
                join user in userRepository.GetAll() on identity.Id equals user.Id
                join role in identityRoleRepository.GetAll() on identity.IdentityRoleId equals role.Id
                where identity.Email == request.Email
                select new
                {
                    User = user,
                    Identity = identity,
                    Role = role.Name
                };
            
            var identityUser = await asyncQueryableExecutor
                .FirstOrDefaultAsync(
                    identityUserQ, 
                    false,
                    cancellationToken);
                
            if (identityUser is null || 
                identityUser.Identity.ValidatePassword(request.Password) is false)
            {
                return Result.Fail(AuthenticationErrorMessages.LoginFailError);
            }

            // This query includes verification information, major information, requests getting class

            //3. Generate token
            var userLoginDto = Mapper.Map<UserLoginDto>(identityUser);
            var loginToken = jwtTokenGenerator.GenerateToken(userLoginDto);

            return new AuthenticationResult()
            {
                User = userLoginDto,
                Token = loginToken,
            };
        }
        catch (Exception ex)
        {
            return Result.Fail($"AccountServiceError.FailToGetTutorProfileWithException {ex.Message}");
        }
    }
}