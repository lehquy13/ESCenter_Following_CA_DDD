using ESCenter.Application.Interfaces.Authentications;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Queries.ValidateToken;

public class ValidateTokenQueryHandler(
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    IJwtTokenGenerator jwtTokenGenerator
)
    : QueryHandlerBase<ValidateTokenQuery>(logger, mapper)
{
    public override async Task<Result> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtTokenGenerator.ValidateToken(request.ValidateToken)
            ? Result.Success()
            : Result.Fail("Token is invalid.");
    }
}