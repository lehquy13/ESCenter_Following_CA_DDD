using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Commands.ChangeAvatar;

public class ChangeAvatarCommandHandler(
    IUserRepository accountRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ChangeAvatarCommandHandler> logger)
    : CommandHandlerBase<ChangeAvatarCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.CurrentUserId is null)
        {
            return Result.Fail(ProfileAppServiceError.UnAuthorized);
        }

        var id = IdentityGuid.Create(new Guid(currentUserService.CurrentUserId));
        
        var account = await accountRepository.GetAsync(id, cancellationToken);

        if (account == null)
        {
            return Result.Fail(AccountServiceError.NonExistUserError);
        }

        account.SetAvatar(request.Url);
        
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(AccountServiceError.FailToUpdateUserErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}

public static class ProfileAppServiceError
{
    public static Error UnAuthorized = new Error("UnAuzthorized", "Somehow they cant authorize");
}