using ESCenter.Application.EventHandlers;
using ESCenter.Application.ServiceImpls.Accounts;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.ServiceImpls.Admins.Users.Commands.CreateUpdateUserProfile;

public class CreateUpdateUserProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    IUserRepository userRepository,
    IUserDomainService userDomainService,
    IPublisher publisher)
    : CommandHandlerBase<CreateUpdateUserProfileCommand>(unitOfWork, logger)
{
    private IMapper Mapper { get; } = mapper;

    private const string DefaultAvatar =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";

    public override async Task<Result> Handle(CreateUpdateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetAsync(
                IdentityGuid.Create(command.LearnerForCreateUpdateDto.Id), cancellationToken);

            // Check if the user existed
            if (user is not null)
            {
                // Update user
                Mapper.Map(command.LearnerForCreateUpdateDto, user);

                if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
                {
                    return Result.Fail(AccountServiceError.FailToUpdateUserErrorWhileSavingChanges);
                }

                return Result.Success();
            }

            // Create new user
            user = await userDomainService.CreateAsync(
                command.LearnerForCreateUpdateDto.FirstName,
                command.LearnerForCreateUpdateDto.LastName,
                command.LearnerForCreateUpdateDto.Gender.ToEnum<Gender>(),
                command.LearnerForCreateUpdateDto.BirthYear,
                Address.Create(
                    command.LearnerForCreateUpdateDto.City,
                    command.LearnerForCreateUpdateDto.Country),
                command.LearnerForCreateUpdateDto.Description,
                DefaultAvatar,
                command.LearnerForCreateUpdateDto.Email,
                command.LearnerForCreateUpdateDto.PhoneNumber,
                UserRole.Learner);

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailToCreateUserErrorWhileSavingChanges);
            }

            var message = $"New learner: {user.FirstName} {user.LastName} at {user.CreationTime.ToLongDateString()}";

            await publisher.Publish(
                new NewObjectCreatedEvent(user.Id.Value.ToString(), message, NotificationEnum.Learner),
                cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(
                UserError.FailTCreateOrUpdateUserError(command.LearnerForCreateUpdateDto.Email, ex.Message));
        }
    }
}