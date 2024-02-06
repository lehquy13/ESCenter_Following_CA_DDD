using ESCenter.Application.Contract.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users.Identities;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.Login;

public class LoginQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<LoginQueryHandler> logger,
    IMapper mapper,
    IIdentityRepository identityRepository,
    IJwtTokenGenerator jwtTokenGenerator
)
    : QueryHandlerBase<LoginQuery, AuthenticationResult>(unitOfWork, logger, mapper)
{
    public override async Task<Result<AuthenticationResult>> Handle(LoginQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var identityUser = await identityRepository
                .FindByEmailAsync(request.Email, cancellationToken);

            if (identityUser is null || identityUser.ValidatePassword(request.Password) is false)
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