using ESCenter.Admin.Application.Contracts.Profiles;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserProfileDto>;