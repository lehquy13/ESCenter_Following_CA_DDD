using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Subscriber;

public record UnSubscribeCommand(string Email) : ICommandRequest;