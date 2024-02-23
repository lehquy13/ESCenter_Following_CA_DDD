using ESCenter.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Commands.UpdateTutorProfile;

public record UpdateTutorInformationCommand(TutorBasicUpdateForClientDto TutorBasicUpdateDto) : ICommandRequest;