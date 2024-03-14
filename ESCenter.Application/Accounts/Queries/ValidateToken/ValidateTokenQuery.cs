using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.Accounts.Queries.ValidateToken;

public record ValidateTokenQuery(
    string ValidateToken
) : IQueryRequest;

