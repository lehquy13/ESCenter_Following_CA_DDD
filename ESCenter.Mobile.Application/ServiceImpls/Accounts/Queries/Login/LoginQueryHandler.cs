using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.DomainServices.Interfaces;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Queries.Login;

public class LoginQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<LoginQueryHandler> logger,
    IMapper mapper,
    IUserRepository userRepository,
    IIdentityDomainServices identityDomainServices,
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
            var identityUserQ = await identityDomainServices.SignInAsync(request.Email, request.Password);

            if (identityUserQ is null)
            {
                return Result.Fail(AuthenticationErrorMessages.LoginFailError);
            }

            var user = await userRepository.GetAsync(identityUserQ.Id, cancellationToken);

            if (user is null)
            {
                return Result.Fail(AuthenticationErrorMessages.LoginFailError);
            }

            //3. Generate token
            var userLoginDto = Mapper.Map<UserLoginDto>(user);
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