using ESCenter.Admin.Application.Contracts.Users.Tutors;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Commands.RegisterAsTutor;

public record RegisterAsTutorCommand(TutorBasicForRegisterCommand TutorBasicForRegisterCommand) : ICommandRequest;
