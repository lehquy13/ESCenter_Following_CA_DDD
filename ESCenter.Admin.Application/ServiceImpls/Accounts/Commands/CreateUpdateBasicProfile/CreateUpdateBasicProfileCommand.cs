using ESCenter.Admin.Application.Contracts.Profiles;
using ESCenter.Application.Contracts.Authentications;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Commands.CreateUpdateBasicProfile;

public record CreateUpdateBasicProfileCommand(
    UserProfileCreateUpdateDto UserProfileCreateUpdateDto
) : ICommandRequest<AuthenticationResult>;