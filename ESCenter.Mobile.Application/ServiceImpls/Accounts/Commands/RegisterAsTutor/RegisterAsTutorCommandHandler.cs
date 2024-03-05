using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Accounts.Commands.RegisterAsTutor;

public class RegisterAsTutorCommandHandler(
    IUnitOfWork unitOfWork,
    IIdentityDomainServices identityDomainServices,
    IAppLogger<RegisterAsTutorCommandHandler> logger)
    : CommandHandlerBase<RegisterAsTutorCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RegisterAsTutorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the user existed
            var result = await identityDomainServices.RegisterAsTutor(
                IdentityGuid.Create(command.TutorBasicForRegisterCommand.Id),
                command.TutorBasicForRegisterCommand.AcademicLevel.ToEnum<AcademicLevel>(),
                command.TutorBasicForRegisterCommand.University,
                command.TutorBasicForRegisterCommand.Majors,
                command.TutorBasicForRegisterCommand.ImageFileUrls);

            if (result.IsFailure)
            {
                return Result.Fail(AccountServiceError.FailRegisteringAsTutorError)
                    .WithError(result.Error);
            }

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailRegisteringAsTutorErrorWhileSavingChanges);
            }

            Logger.LogInformation("Done registering tutor");
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Fail(AccountServiceError.FailRegisteringAsTutorErrorWhileSavingChanges + e.Message);
        }
    }
}