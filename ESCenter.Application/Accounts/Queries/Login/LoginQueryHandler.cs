using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Queries.Login;

public class LoginQueryHandler(
    IAppLogger<LoginQueryHandler> logger,
    IMapper mapper,
    IIdentityService identityService,
    IJwtTokenGenerator jwtTokenGenerator
)
    : QueryHandlerBase<LoginQuery, AuthenticationResult>(logger, mapper)
{
    public override async Task<Result<AuthenticationResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.SignInAsync(
            request.Email, request.Password, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var customer = result.Value;

        //3. Generate token
        var userLoginDto = new UserLoginDto()
        {
            Id = customer.Id.Value,
            Email = customer.Email,
            FullName = $"{customer.FirstName} {customer.LastName}",
            Role = customer.Role.ToString(),
            Avatar = customer.Avatar
        };
        var loginToken = jwtTokenGenerator.GenerateToken(userLoginDto);

        return new AuthenticationResult()
        {
            User = userLoginDto,
            Token = loginToken,
        };
    }
}