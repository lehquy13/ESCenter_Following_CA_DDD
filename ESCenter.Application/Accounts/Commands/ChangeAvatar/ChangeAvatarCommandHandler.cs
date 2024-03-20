using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ChangeAvatar;

public class ChangeAvatarCommandHandler(
    ICustomerRepository accountRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<ChangeAvatarCommandHandler> logger)
    : CommandHandlerBase<ChangeAvatarCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        var id = CustomerId.Create(currentUserService.UserId);

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