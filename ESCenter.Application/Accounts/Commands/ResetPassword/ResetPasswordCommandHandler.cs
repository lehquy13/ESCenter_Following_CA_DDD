using ESCenter.Domain.Aggregates.Users;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ResetPassword;

internal class ResetPasswordCommandHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
    IAppLogger<ResetPasswordCommandHandler> logger)
    : CommandHandlerBase<ResetPasswordCommand>(unitOfWork, logger)

{
    public override async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService
            .ResetPasswordAsync(
                command.Email,
                command.NewPassword,
                command.Otp);

        if (!result.IsSuccess || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            logger.LogError( "Reset password fail", result.Error.ToString());
            return AuthenticationErrorMessages.ResetPasswordFail;
        }

        return Result.Success();
    }
}