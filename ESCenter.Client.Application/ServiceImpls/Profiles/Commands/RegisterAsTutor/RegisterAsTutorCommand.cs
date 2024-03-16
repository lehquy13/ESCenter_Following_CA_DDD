using ESCenter.Client.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorRegistrationDto TutorRegistrationDto) : ICommandRequest;
