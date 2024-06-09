using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices;

public class UserDomainService(
    IAppLogger<UserDomainService> logger,
    ICustomerRepository customerRepository,
    IIdentityService identityService
) : DomainServiceBase(logger), IUserDomainService
{
    public async Task<Result<Customer>> CreateAsync(
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber,
        Role role,
        CancellationToken cancellationToken = default)
    {
        var customer = await identityService.CreateAsync(
            string.Empty,
            firstName,
            lastName,
            gender,
            birthYear,
            address,
            description,
            avatar,
            email,
            phoneNumber,
            role, cancellationToken);

        if (customer.IsFailure)
        {
            return customer;
        }

        await customerRepository.InsertAsync(customer.Value, cancellationToken);

        return customer;
    }
}