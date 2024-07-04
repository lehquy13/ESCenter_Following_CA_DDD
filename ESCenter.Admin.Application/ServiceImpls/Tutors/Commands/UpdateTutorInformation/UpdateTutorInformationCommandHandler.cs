using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorInformation;

public class UpdateTutorInformationCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateTutorInformationCommandHandler> logger,
    ITutorRepository tutorRepository,
    IMapper mapper)
    : CommandHandlerBase<UpdateTutorInformationCommand>(unitOfWork,
        logger)
{
    public override async Task<Result> Handle(UpdateTutorInformationCommand command,
        CancellationToken cancellationToken)
    {
        var tutorId = CustomerId.Create(command.TutorBasicUpdateDto.Id);
        var tutor = await tutorRepository.GetTutorByUserId(tutorId, cancellationToken);

        // Check if the tutor exist
        if (tutor is null)
        {
            return Result.Fail(UserError.NonExistTutorError);
        }

        mapper.Map(command.TutorBasicUpdateDto, tutor);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(TutorAppServiceError.FailToUpdateTutorWhileSavingChanges);
        }

        return Result.Success();
    }
}