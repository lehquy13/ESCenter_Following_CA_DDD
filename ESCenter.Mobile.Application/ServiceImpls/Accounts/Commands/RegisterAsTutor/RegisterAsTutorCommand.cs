using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorBasicForRegisterCommand TutorBasicForRegisterCommand) : ICommandRequest;
