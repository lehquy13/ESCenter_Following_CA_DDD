using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Contracts.Profiles;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.Accounts.Commands.CreateUpdateBasicProfile;

public record CreateUpdateBasicProfileCommand(
    UserProfileCreateUpdateDto UserProfileCreateUpdateDto
) : ICommandRequest<AuthenticationResult>;