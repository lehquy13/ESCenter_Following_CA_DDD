using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Errors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.ForgetPassword;

public record ConfirmEmailCommand(
    Guid UserId,
    string Token
) : ICommandRequest;

public class ConfirmEmailCommandHandler(
    IIdentityService identityService,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<ConfirmEmailCommandHandler> logger
) : CommandHandlerBase<ConfirmEmailCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService.ConfirmEmail(command.UserId.ToString(), command.Token);

        if (!result.IsSuccess)
        {
            return Result.Fail(result.Error);
        }

        var customer = await customerRepository.GetAsync(CustomerId.Create(command.UserId), cancellationToken);
        
        if (customer is null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }
        
        customer.VerifyEmail();
        
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}