using ESCenter.Application.Contract.Authentications;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IQueryRequest<AuthenticationResult>;