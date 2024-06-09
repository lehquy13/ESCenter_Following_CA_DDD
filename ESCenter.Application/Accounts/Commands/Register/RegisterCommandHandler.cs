using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.Accounts.Commands.Register;

public class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RegisterCommandHandler> logger,
    IIdentityService identityService,
    ICustomerRepository customerRepository
)
    : CommandHandlerBase<RegisterCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService.CreateAsync(command.Username,
            command.FirstName,
            command.LastName,
            command.Gender,
            command.BirthYear,
            Address.Create(command.City,
                command.Country),
            string.Empty,
            string.Empty,
            command.Email,
            command.PhoneNumber,
            cancellationToken: cancellationToken);

        if (!result.IsSuccess)
        {
            return Result.Fail(result.Error);
        }

        await customerRepository.InsertAsync(result.Value, cancellationToken);

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail(AuthenticationErrorMessages.CreateUserFailWhileSavingChanges)
            : Result.Success();
    }
}