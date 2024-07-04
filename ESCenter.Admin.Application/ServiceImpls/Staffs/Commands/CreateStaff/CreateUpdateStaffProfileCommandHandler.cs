using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Staffs.Commands.CreateStaff;

public class CreateUpdateStaffProfileCommandHandler(
    ICustomerRepository customerRepository,
    IStaffDomainService staffDomainService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateUpdateStaffProfileCommandHandler> logger,
    IMapper mapper)
    : CommandHandlerBase<CreateUpdateStaffProfileCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateUpdateStaffProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await customerRepository.GetAsync(
            CustomerId.Create(command.LearnerForCreateUpdateDto.Id), cancellationToken);

        // Check if the user existed
        if (user is not null)
        {
            // Update user
            mapper.Map(command.LearnerForCreateUpdateDto, user);

            return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
                ? Result.Fail(StaffAppServiceError.FailToUpdateStaffErrorWhileSavingChanges)
                : Result.Success();
        }

        // Create new user
        var staff = await staffDomainService.CreateStaff(
            string.Empty,
            command.LearnerForCreateUpdateDto.FirstName,
            command.LearnerForCreateUpdateDto.LastName,
            command.LearnerForCreateUpdateDto.Gender.ToEnum<Gender>(),
            command.LearnerForCreateUpdateDto.BirthYear,
            Address.Create(
                command.LearnerForCreateUpdateDto.City,
                command.LearnerForCreateUpdateDto.Country),
            command.LearnerForCreateUpdateDto.Description,
            "",
            command.LearnerForCreateUpdateDto.Email,
            command.LearnerForCreateUpdateDto.PhoneNumber);

        if (staff.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return staff.IsFailure
                ? staff.Error
                : Result.Fail(StaffAppServiceError.FailToCreateStaffErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}