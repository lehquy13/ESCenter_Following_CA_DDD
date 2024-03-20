using ESCenter.Domain.Aggregates.Users;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ForgetPassword;

internal class ForgetPasswordCommandHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
    IAppLogger<ForgetPasswordCommandHandler> logger)
    : CommandHandlerBase<ForgetPasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ForgetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService.ForgetPasswordAsync(command.Email);

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.ResetPasswordFail);
        }

        return Result.Success();
    }
}