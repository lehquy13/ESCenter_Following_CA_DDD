using ESCenter.Mobile.Application.Contracts.Profiles;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery() : IQueryRequest<UserProfileDto>;