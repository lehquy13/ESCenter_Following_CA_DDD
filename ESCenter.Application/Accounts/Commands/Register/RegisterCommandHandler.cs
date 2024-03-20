using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
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
        var result = await identityService.CreateAsync(
            command.Username,
            command.FirstName,
            command.LastName,
            command.Gender,
            command.BirthYear,
            Address.Create(command.City, command.Country),
            command.Country,
            string.Empty,
            command.Email,
            command.PhoneNumber);

        if (!result.IsSuccess)
        {
            return Result.Fail(result.Error);
        }

        var user = Customer.Create(
            result.Value.Id,
            command.FirstName,
            command.LastName,
            command.Gender,
            command.BirthYear,
            Address.Create(command.City, command.Country),
            string.Empty,
            string.Empty,
            command.Email,
            command.PhoneNumber,
            UserRole.Learner
        );

        await customerRepository.InsertAsync(user, cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.CreateUserFailWhileSavingChanges);
        }

        return Result.Success();
    }
}