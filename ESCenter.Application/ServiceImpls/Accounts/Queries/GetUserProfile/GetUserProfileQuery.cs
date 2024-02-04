using ESCenter.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserDto>;