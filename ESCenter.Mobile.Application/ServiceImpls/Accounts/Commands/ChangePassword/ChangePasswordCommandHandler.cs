using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.ChangePassword;

internal class ChangePasswordCommandHandler(
        IIdentityDomainServices identityDomainServices,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IAppLogger<ChangePasswordCommandHandler> logger)
    : CommandHandlerBase<ChangePasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(currentUserService.CurrentUserId))
        {
            return Result.Fail(AccountServiceError.UnauthorizedError);
        }
        
        var identityId = IdentityGuid.Create(new Guid(currentUserService.CurrentUserId));
        var result = await identityDomainServices
            .ChangePasswordAsync(
                identityId,
                command.CurrentPassword,
                command.NewPassword);

        if (!result.IsSuccess)
        {
            var resultToReturn = Result.Fail(AuthenticationErrorMessages.ChangePasswordFail)
                .WithError(result.Error);
            return resultToReturn;
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            Logger.LogWarning("Change password fail while saving changes");
            return Result.Fail(AuthenticationErrorMessages.ChangePasswordFailWhileSavingChanges);
        }

        return Result.Success();
    }
}