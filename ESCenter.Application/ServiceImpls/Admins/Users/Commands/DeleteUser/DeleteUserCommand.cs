using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : ICommandRequest;