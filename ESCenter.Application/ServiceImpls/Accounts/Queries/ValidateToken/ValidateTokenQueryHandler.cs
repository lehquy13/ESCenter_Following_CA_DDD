using ESCenter.Application.Contracts.Interfaces.Authentications;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.ValidateToken;

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
        if (jwtTokenGenerator.ValidateToken(request.ValidateToken))
        {
            return Result.Success();
        }

        return Result.Fail("Token is invalid.");
    }
}
