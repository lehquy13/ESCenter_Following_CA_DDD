using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;

namespace ESCenter.Domain.DomainModels.Users;

public record UserProfileDomainModel(IdentityUser IdentityUser, User User);