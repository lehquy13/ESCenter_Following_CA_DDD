using ESCenter.Application.Accounts.Commands.UpdateBasicProfile;
using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.EventHandlers;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.Accounts.Commands.CreateUpdateBasicProfile;

public class UpdateUserProfileCommandHandler( // Change name to Update only
    ICustomerRepository customerRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<UpdateUserProfileCommandHandler> logger,
    IPublisher publisher)
    : CommandHandlerBase<UpdateBasicProfileCommand,
        AuthenticationResult>(unitOfWork, logger)
{
    private IMapper Mapper { get; } = mapper;

    public override async Task<Result<AuthenticationResult>> Handle(UpdateBasicProfileCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await customerRepository.GetAsync(
                CustomerId.Create(currentUserService.UserId),
                cancellationToken);

            // Check if the user existed
            if (user is not null)
            {
                // Update user
                Mapper.Map(command.UserProfileUpdateDto, user);

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
            user = Mapper.Map<Customer>(command.UserProfileUpdateDto);

            await customerRepository.InsertAsync(user, cancellationToken);

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailToCreateUserErrorWhileSavingChanges);
            }

            // TODO: Publish event
            var message = "New learner: " + user.FirstName + " " + user.LastName + " at " +
                          user.CreationTime.ToLongDateString();
            await publisher.Publish(
                new NewObjectCreatedEvent(user.Id.Value.ToString(), message, NotificationEnum.Learner),
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
            return Result.Fail(
                UserError.FailTCreateOrUpdateUserError(command.UserProfileUpdateDto.Email, ex.Message));
        }
    }
}