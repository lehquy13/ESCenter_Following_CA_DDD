using ESCenter.Application.Contracts.Authentications;
using ESCenter.Mobile.Application.Contracts.Profiles;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.CreateUpdateBasicProfile;

public record CreateUpdateBasicProfileCommand(
    UserProfileCreateUpdateDto UserProfileCreateUpdateDto
) : ICommandRequest<AuthenticationResult>;