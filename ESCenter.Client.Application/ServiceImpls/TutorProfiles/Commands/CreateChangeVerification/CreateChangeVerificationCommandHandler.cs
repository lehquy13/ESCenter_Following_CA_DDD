using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Specifications.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.TutorProfiles.Commands.CreateChangeVerification;

public class CreateChangeVerificationCommandHandler(
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateChangeVerificationCommandHandler> logger)
    : CommandHandlerBase<CreateChangeVerificationCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateChangeVerificationCommand command,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(
            new TutorByCustomerIdSpec(CustomerId.Create(currentUserService.UserId)), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        tutor.CreateChangeVerificationRequest(command.ImageUrls);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}