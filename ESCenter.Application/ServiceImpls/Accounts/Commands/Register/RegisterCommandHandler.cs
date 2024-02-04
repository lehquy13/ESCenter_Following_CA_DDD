using ESCenter.Domain.DomainServices.Interfaces;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.Register;

public class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RegisterCommandHandler> logger,
    IIdentityDomainServices identityDomainServices
)
    : CommandHandlerBase<RegisterCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await identityDomainServices.CreateAsync(
            command.Username,
            command.Email,
            command.Password,
            command.PhoneNumber
        );

        if (!result.IsSuccess || result.Value is null)
        {
            var resultToReturn = Result.Fail(AuthenticationErrorMessages.RegisterFail)
                .WithError(result.Error);
            return resultToReturn;
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) < 0)
        {
            return Result.Fail(AuthenticationErrorMessages.CreateUserFailWhileSavingChanges);
        }

        return Result.Success();
    }
}