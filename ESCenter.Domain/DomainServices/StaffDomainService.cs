using ESCenter.Domain.Aggregates.Staff;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Errors;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Domain.DomainServices;

public class StaffDomainService(
    IRepository<Staff, StaffId> staffRepository,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IStaffDomainService
{
    public async Task<Result> CreateStaff(
        string userName,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber
    )
    {
        if (!currentUserService.IsAuthenticated)
        {
            return Result.Fail(DomainServiceErrors.Unauthorized);
        }

        if (!currentUserService.Roles.Contains("Admin"))
        {
            return Result.Fail(DomainServiceErrors.Incompetent);
        }

        var userInformation = await identityService.CreateAsync(
            string.Empty,
            firstName,
            lastName,
            gender,
            birthYear,
            address,
            description,
            string.Empty,
            email,
            phoneNumber);

        if (userInformation.IsFailure)
        {
            return userInformation.Error;
        }

        var staff = Staff.Create(userInformation.Value.Id.Value);

        await staffRepository.InsertAsync(staff);

        return Result.Success();
    }
}

public interface IStaffDomainService : IDomainService
{
    Task<Result> CreateStaff(
        string userName,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber
    );
}