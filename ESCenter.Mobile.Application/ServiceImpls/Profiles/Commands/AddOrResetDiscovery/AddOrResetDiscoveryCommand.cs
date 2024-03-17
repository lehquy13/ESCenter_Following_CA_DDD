using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.AddOrResetDiscovery;

public record AddOrResetDiscoveryCommand(Guid UserId, List<Guid> DiscoveryIds) : ICommandRequest, IAuthorizationRequest;

public class AddOrResetDiscoveryCommandValidator : AbstractValidator<AddOrResetDiscoveryCommand>
{
    public AddOrResetDiscoveryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DiscoveryIds).NotEmpty();
    }
}
