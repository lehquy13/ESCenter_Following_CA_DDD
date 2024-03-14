using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ResetPassword;

internal class ResetPasswordCommandHandler(
    IIdentityDomainServices identityDomainServices,
    IUnitOfWork unitOfWork,
    IAppLogger<ResetPasswordCommandHandler> logger)
    : CommandHandlerBase<ResetPasswordCommand>(unitOfWork, logger)

{
    public override async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await identityDomainServices
            .ResetPasswordAsync(
                command.Email,
                command.NewPassword,
                command.Otp);

        if (!result.IsSuccess)
        {
            return Result.Fail(result.Error);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return AuthenticationErrorMessages.ResetPasswordFailWhileSavingChanges;
        }

        return Result.Success();
    }
}