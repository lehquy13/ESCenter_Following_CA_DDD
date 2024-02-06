using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.UpdateTutorProfile;

public record UpdateTutorProfileCommand
(
    TutorBasicForUpdateDto TutorBasicForUpdateDto
) : ICommandRequest;