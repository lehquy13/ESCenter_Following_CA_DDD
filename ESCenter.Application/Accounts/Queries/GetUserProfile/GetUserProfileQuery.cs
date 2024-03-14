using ESCenter.Application.Contracts.Profiles;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserProfileDto>;