using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.ChangeAvatar;

public record ChangeAvatarCommand(Guid UserId, string Url) : ICommandRequest;