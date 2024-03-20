using ESCenter.Application.EventHandlers;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.CreateTutor;

public class CreateTutorCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IIdentityService identityService,
    ITutorDomainService tutorDomainService,
    IPublisher publisher)
    : CommandHandlerBase<CreateTutorCommand>(unitOfWork,
        logger)
{
    public override async Task<Result> Handle(CreateTutorCommand command, CancellationToken cancellationToken)
    {
        //Register user as new tutor
        var userInformation = await identityService.CreateAsync(
            string.Empty,
            command.TutorForCreateDto.LearnerForCreateUpdateDto.FirstName,
            command.TutorForCreateDto.LearnerForCreateUpdateDto.LastName,
            command.TutorForCreateDto.LearnerForCreateUpdateDto.Gender.ToEnum<Gender>(),
            command.TutorForCreateDto.LearnerForCreateUpdateDto.BirthYear,
            Address.Create(
                command.TutorForCreateDto.LearnerForCreateUpdateDto.City,
                command.TutorForCreateDto.LearnerForCreateUpdateDto.Country),
            command.TutorForCreateDto.LearnerForCreateUpdateDto.Description,
            string.Empty,
            command.TutorForCreateDto.LearnerForCreateUpdateDto.Email,
            command.TutorForCreateDto.LearnerForCreateUpdateDto.PhoneNumber,
            UserRole.Tutor, cancellationToken);

        if (userInformation.IsFailure)
        {
            return userInformation.Error;
        }

        var tutorInformation = await tutorDomainService.CreateTutorWithEmptyVerificationAsync(
            userInformation.Value.Id,
            command.TutorForCreateDto.TutorProfileCreateDto.AcademicLevel.ToEnum<AcademicLevel>(),
            command.TutorForCreateDto.TutorProfileCreateDto.University,
            command.TutorForCreateDto.TutorProfileCreateDto.MajorIds,
            command.TutorForCreateDto.TutorProfileCreateDto.IsVerified);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(TutorAppServiceError.FailToCreateTutorWhileSavingChanges);
        }

        var message = $"New tutor: {tutorInformation.Id.Value} at {DateTime.Now.ToLongDateString()}";
        await publisher.Publish(
            new NewObjectCreatedEvent(userInformation.Value.Id.Value.ToString(), message, NotificationEnum.Tutor),
            cancellationToken);

        return Result.Success();
    }
}