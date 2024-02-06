using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.EventHandlers;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.CreateUpdateLearnerProfile;

public class CreateUpdateLearnerProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IPublisher publisher)
    : CommandHandlerBase<CreateUpdateLearnerProfileCommand,
        AuthenticationResult>(unitOfWork, logger)
{
    private IMapper Mapper { get; } = mapper;

    public override async Task<Result<AuthenticationResult>> Handle(CreateUpdateLearnerProfileCommand command,
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

                //3. Generate token
                var userLoginForUpdateDto = Mapper.Map<UserLoginDto>(user);
                var loginToken = jwtTokenGenerator.GenerateToken(userLoginForUpdateDto);

                return new AuthenticationResult()
                {
                    User = userLoginForUpdateDto,
                    Token = loginToken,
                };
            }

            //Create new user
            user = Mapper.Map<User>(command.LearnerForCreateUpdateDto);

            await userRepository.InsertAsync(user, cancellationToken);
            
            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailToCreateUserErrorWhileSavingChanges);
            }

            // TODO: Publish event
            var message = "New learner: " + user.FirstName + " " + user.LastName + " at " +
                          user.CreationTime.ToLongDateString();
            await publisher.Publish(new NewObjectCreatedEvent(user.Id.Value.ToString(), message, NotificationEnum.Learner),
                cancellationToken);
            //3. Generate token
            var userLoginForCreateDto = Mapper.Map<UserLoginDto>(user);
            var loginTokenForCreate = jwtTokenGenerator.GenerateToken(userLoginForCreateDto);

            return new AuthenticationResult()
            {
                User = userLoginForCreateDto,
                Token = loginTokenForCreate,
            };
        }
        catch (Exception ex)
        {
            return Result.Fail(UserError.FailTCreateOrUpdateUserError(command.LearnerForCreateUpdateDto.Email, ex.Message));
        }
    }
}