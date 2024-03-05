using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;

public record AddOrResetDiscoveryCommand(Guid UserId, List<Guid> DiscoveryIds) : ICommandRequest;