using ESCenter.Application.Contract.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorBasicForRegisterCommand TutorBasicForRegisterCommand) : ICommandRequest;
