using ESCenter.Application.Contract.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserDto>;