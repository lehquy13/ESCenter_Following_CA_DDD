using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.UpdateTutorProfile;

public record UpdateTutorInformationCommand(TutorBasicUpdateForClientDto TutorBasicUpdateDto) : ICommandRequest;