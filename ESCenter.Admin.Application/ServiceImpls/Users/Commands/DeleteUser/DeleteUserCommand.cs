using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : ICommandRequest;