using ESCenter.Application.Contract.Authentications;
using ESCenter.Application.Contract.Users.Learners;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateLearnerProfile;

public record CreateUpdateLearnerProfileCommand(
    LearnerForCreateUpdateDto LearnerForCreateUpdateDto
) : ICommandRequest<AuthenticationResult>;