using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Domain;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.Accounts.Commands.UpdateBasicProfile;

public class UpdateBasicProfileCommandHandler( // Change name to Update only
    ICustomerRepository customerRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<UpdateBasicProfileCommandHandler> logger,
    IPublisher publisher)
    : CommandHandlerBase<UpdateBasicProfileCommand,
        AuthenticationResult>(unitOfWork, logger)
{
    public override async Task<Result<AuthenticationResult>> Handle(UpdateBasicProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await customerRepository.GetAsync(
            CustomerId.Create(currentUserService.UserId),
            cancellationToken);

        // Check if the user existed
        if (user is not null)
        {
            // Update user
            mapper.Map(command.UserProfileUpdateDto, user);

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailToUpdateUserErrorWhileSavingChanges);
            }

            //3. Generate token
            var userLoginForUpdateDto = mapper.Map<UserLoginDto>(user);
            var loginToken = jwtTokenGenerator.GenerateToken(userLoginForUpdateDto);

            return new AuthenticationResult()
            {
                User = userLoginForUpdateDto,
                Token = loginToken,
            };
        }

        //Create new user
        user = mapper.Map<Customer>(command.UserProfileUpdateDto);

        await customerRepository.InsertAsync(user, cancellationToken);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(AccountServiceError.FailToCreateUserErrorWhileSavingChanges);
        }

        // TODO: Publish event
        var message = "New learner: " + user.FirstName + " " + user.LastName + " at " +
                      user.CreationTime.ToLongDateString();
        await publisher.Publish(
            new NewDomainObjectCreatedEvent(user.Id.Value.ToString(), message, NotificationEnum.Learner),
            cancellationToken);
        
        //3. Generate token
        var userLoginForCreateDto = mapper.Map<UserLoginDto>(user);
        var loginTokenForCreate = jwtTokenGenerator.GenerateToken(userLoginForCreateDto);

        return new AuthenticationResult
        {
            User = userLoginForCreateDto,
            Token = loginTokenForCreate,
        };
    }
}