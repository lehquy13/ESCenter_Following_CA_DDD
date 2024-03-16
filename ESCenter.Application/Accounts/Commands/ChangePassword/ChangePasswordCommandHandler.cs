using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ChangePassword;

internal class ChangePasswordCommandHandler(
    IIdentityDomainServices identityDomainServices,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ChangePasswordCommandHandler> logger)
    : CommandHandlerBase<ChangePasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var identityId = IdentityGuid.Create(currentUserService.UserId);
        var result = await identityDomainServices
            .ChangePasswordAsync(
                identityId,
                command.CurrentPassword,
                command.NewPassword);

        if (!result.IsSuccess)
        {
            return Result.Fail(result.Error);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            Logger.LogWarning("Change password fail while saving changes");
            return Result.Fail(AuthenticationErrorMessages.ChangePasswordFailWhileSavingChanges);
        }

        return Result.Success();
    }
}