using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;

public record AddOrResetDiscoveryCommand(Guid UserId, List<Guid> DiscoveryIds) : ICommandRequest;