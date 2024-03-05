using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Queries.ValidateToken;

public record ValidateTokenQuery(
    string ValidateToken
) : IQueryRequest;

