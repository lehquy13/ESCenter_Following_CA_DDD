using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Accounts.Commands.Register;

public class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RegisterCommandHandler> logger,
    IIdentityDomainServices identityDomainServices,
    IUserRepository userRepository
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

        var user = User.Create(
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
        
        await userRepository.InsertAsync(user, cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.CreateUserFailWhileSavingChanges);
        }

        return Result.Success();
    }
}