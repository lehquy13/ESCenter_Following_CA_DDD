using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Profiles.Commands.RegisterAsTutor;

public class RegisterAsTutorCommandHandler(
    IUnitOfWork unitOfWork,
    IIdentityDomainServices identityDomainServices,
    ICurrentUserService currentUserService,
    IAppLogger<RegisterAsTutorCommandHandler> logger)
    : CommandHandlerBase<RegisterAsTutorCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RegisterAsTutorCommand command, CancellationToken cancellationToken)
    {
        // Check if the user existed
        var result = await identityDomainServices.RegisterAsTutor(
            IdentityGuid.Create(currentUserService.UserId),
            command.TutorRegistrationDto.AcademicLevel.ToEnum<AcademicLevel>(),
            command.TutorRegistrationDto.University,
            command.TutorRegistrationDto.Majors,
            command.TutorRegistrationDto.ImageFileUrls);

        if (result.IsFailure)
        {
            return Result.Fail(result.Error);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(UserAppServiceError.FailRegisteringAsTutorErrorWhileSavingChanges);
        }

        Logger.LogInformation("Done registering tutor");
        return Result.Success();
    }
}