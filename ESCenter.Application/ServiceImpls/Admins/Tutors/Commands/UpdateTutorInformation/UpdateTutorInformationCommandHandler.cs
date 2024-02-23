using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Specifications.Subjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.UpdateTutorInformation;

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
        try
        {
            var tutorId = TutorId.Create(command.TutorBasicUpdateDto.Id);
            var tutor = await tutorRepository.GetAsync(tutorId, cancellationToken);

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
        catch (Exception ex)
        {
            return Result.Fail("Error happens when tutor is adding or updating: " + ex.Message);
        }
    }
}