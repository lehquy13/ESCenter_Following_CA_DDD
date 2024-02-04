using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.ChangeAvatar;

public class ChangeAvatarCommandHandler(
    IUserRepository accountRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<ChangeAvatarCommandHandler> logger)
    : CommandHandlerBase<ChangeAvatarCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(IdentityGuid.Create(request.UserId), cancellationToken);

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