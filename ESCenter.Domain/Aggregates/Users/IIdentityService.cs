using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.Aggregates.Users;

public interface IIdentityService : IDomainService
{
    /// <summary>
    /// Sign in with email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Customer?> SignInAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new and empty user with default value: email, password, phoneNumber
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="gender"></param>
    /// <param name="birthYear"></param>
    /// <param name="address"></param>
    /// <param name="description"></param>
    /// <param name="avatar"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<Customer>> CreateAsync(
        string userName,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber,
        UserRole role = UserRole.Learner,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<Result> ChangePasswordAsync(CustomerId customerId, string currentPassword, string newPassword);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="newPassword"></param>
    /// <param name="otpCode"></param>
    /// <returns></returns>
    Task<Result> ResetPasswordAsync(string email, string newPassword, string otpCode);
    Task<Result> RegisterAsTutor(
        CustomerId customerId,
        AcademicLevel academicLevel,
        string university,
        List<int> majors,
        List<string> verificationInfoDtos,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<Result> RemoveAsync(CustomerId customerId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandEmail"></param>
    /// <returns></returns>
    Task<Result> ForgetPasswordAsync(string commandEmail);
}