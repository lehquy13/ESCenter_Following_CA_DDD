﻿using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ChangePassword;

internal class ChangePasswordCommandHandler(
    IIdentityService identityService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ChangePasswordCommandHandler> logger)
    : CommandHandlerBase<ChangePasswordCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService
            .ChangePasswordAsync(
                CustomerId.Create(currentUserService.UserId),
                command.CurrentPassword,
                command.NewPassword);

        // It is not necessary to save changes here because the identity service will save the changes
        if (!result.IsSuccess)
        {
            Logger.LogWarning("Change password fail", result.Error.ToString());
            return Result.Fail(AuthenticationErrorMessages.ChangePasswordFail);
        }

        return Result.Success();
    }
}