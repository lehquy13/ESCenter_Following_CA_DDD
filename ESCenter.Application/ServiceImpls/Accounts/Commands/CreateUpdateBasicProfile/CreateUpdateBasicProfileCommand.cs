using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Contracts.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateBasicProfile;

public record CreateUpdateBasicProfileCommand(
    LearnerForCreateUpdateDto LearnerForCreateUpdateDto
) : ICommandRequest<AuthenticationResult>;