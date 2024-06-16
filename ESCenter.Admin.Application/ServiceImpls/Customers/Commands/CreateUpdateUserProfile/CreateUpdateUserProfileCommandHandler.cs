using ESCenter.Domain;
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

namespace ESCenter.Admin.Application.ServiceImpls.Customers.Commands.CreateUpdateUserProfile;

public class CreateUpdateUserProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    ICustomerRepository customerRepository,
    IUserDomainService userDomainService,
    IPublisher publisher
) : CommandHandlerBase<CreateUpdateUserProfileCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateUpdateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await customerRepository.GetAsync(
                CustomerId.Create(command.LearnerForCreateUpdateDto.Id), cancellationToken);

            // Check if the user existed
            if (user is not null)
            {
                // Update user
                mapper.Map(command.LearnerForCreateUpdateDto, user);

                return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
                    ? Result.Fail(UserAppServiceError.FailToUpdateUserErrorWhileSavingChanges)
                    : Result.Success();
            }

            // Create new user
            var result = await userDomainService.CreateAsync(
                command.LearnerForCreateUpdateDto.FirstName,
                command.LearnerForCreateUpdateDto.LastName,
                command.LearnerForCreateUpdateDto.Gender.ToEnum<Gender>(),
                command.LearnerForCreateUpdateDto.BirthYear,
                Address.Create(
                    command.LearnerForCreateUpdateDto.City,
                    command.LearnerForCreateUpdateDto.Country),
                command.LearnerForCreateUpdateDto.Description,
                command.LearnerForCreateUpdateDto.Avatar,
                command.LearnerForCreateUpdateDto.Email,
                command.LearnerForCreateUpdateDto.PhoneNumber,
                Role.Learner, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error;
            }

            user = result.Value;

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(UserAppServiceError.FailToCreateUserErrorWhileSavingChanges);
            }

            var message = $"New learner: {user.FirstName} {user.LastName} at {user.CreationTime.ToLongDateString()}";

            await publisher.Publish(
                new NewDomainObjectCreatedEvent(user.Id.Value.ToString(), message, NotificationEnum.Learner),
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