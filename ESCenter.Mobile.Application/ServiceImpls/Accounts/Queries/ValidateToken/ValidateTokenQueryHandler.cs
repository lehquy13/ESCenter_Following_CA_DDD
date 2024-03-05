using ESCenter.Application.Interfaces.Authentications;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Queries.ValidateToken;

public class ValidateTokenQueryHandler(
    IUnitOfWork unitOfWork, 
    IAppLogger<RequestHandlerBase> logger, 
    IMapper mapper,
    IJwtTokenGenerator jwtTokenGenerator
    )
    : QueryHandlerBase<ValidateTokenQuery>(unitOfWork, logger, mapper)
{
    public override async Task<Result> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return jwtTokenGenerator.ValidateToken(request.ValidateToken) ? Result.Success() : Result.Fail("Token is invalid.");
    }
}
