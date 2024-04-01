using System.Text;
using System.Text.Encodings.Web;
using Azure.Core;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices;
using ESCenter.Domain.DomainServices.Errors;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Specifications.Customers;
using ESCenter.Persistence.EntityFrameworkCore;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace ESCenter.Persistence.Persistence.Repositories;

public class IdentityService(
    SignInManager<EsIdentityUser> signInManager,
    UserManager<EsIdentityUser> userManager,
    RoleManager<EsIdentityRole> roleManager,
    IUserStore<EsIdentityUser> userStore,
    IEmailSender emailSender,
    IAppLogger<IdentityService> logger,
    AppDbContext appDbContext)
    : DomainServiceBase(logger), IIdentityService
{
    public async Task<Customer?> SignInAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var identityUser = await userManager.FindByEmailAsync(email);

        if (identityUser is null)
        {
            return null;
        }

        var result = await signInManager.CheckPasswordSignInAsync(identityUser, password, false);

        if (result.Succeeded)
        {
            return await appDbContext.Customers.FindAsync(
                CustomerId.Create(new Guid(identityUser.Id)),
                cancellationToken);
        }

        return null;
    }

    public async Task<Result<Customer>> CreateAsync(
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
        Role role = Role.Learner,
        CancellationToken cancellationToken = default
    )
    {
        var esIdentityUser = await userManager
            .FindByNameAsync(userName);

        if (esIdentityUser is not null)
        {
            return Result.Fail(DomainServiceErrors.UserAlreadyExistDomainError);
        }

        esIdentityUser = InitializeUserInstance();

        var result = await CreateUser(email, phoneNumber, cancellationToken, esIdentityUser);

        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.First().Description);
        }

        logger.LogInformation("User created a new account with password.");

        var userId = new Guid(await userManager.GetUserIdAsync(esIdentityUser));
        var code = await userManager.GenerateEmailConfirmationTokenAsync(esIdentityUser);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // var callbackUrl = Url.Page(
        //     "/Account/ConfirmEmail",
        //     pageHandler: null,
        //     values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //     protocol: Request.Scheme);

        await emailSender.SendEmail(email, "Demo email",
            $"This email will use to confirm your email using the code {code}");

        //await emailSender.SendHtmlEmail(email, "Confirm your email",
        //  $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        await userManager.AddToRoleAsync(esIdentityUser, role.ToString());

        var customer = Customer.Create(
            CustomerId.Create(userId),
            firstName,
            lastName,
            gender,
            birthYear,
            address,
            description,
            avatar,
            email,
            phoneNumber,
            Role.Learner
        );

        return customer;
    }

    private async Task<IdentityResult> CreateUser(string email, string phoneNumber, CancellationToken cancellationToken,
        EsIdentityUser esIdentityUser)
    {
        await userStore.SetUserNameAsync(esIdentityUser, email, cancellationToken);
        esIdentityUser.Email = email;
        esIdentityUser.NormalizedEmail = email.ToUpper();
        esIdentityUser.PhoneNumber = phoneNumber;

        //await emailStore.SetEmailAsync(esIdentityUser, email, cancellationToken);
        //await phoneNumberStore.SetPhoneNumberAsync(esIdentityUser, phoneNumber, cancellationToken);

        var result = await userManager.CreateAsync(esIdentityUser, DefaultPassword);
        return result;
    }


    private const string DefaultPassword = "1q2w3E**";

    public async Task<Result> ChangePasswordAsync(CustomerId customerId, string currentPassword, string newPassword)
    {
        var identityUser = await userManager.FindByIdAsync(customerId.Value.ToString());

        if (identityUser is null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        var verifyResult = await userManager.ChangePasswordAsync(identityUser, currentPassword, newPassword);

        if (!verifyResult.Succeeded)
        {
            logger.LogError("Fail to change password for user with id {userId} {Message}", customerId,
                verifyResult.Errors.Select(x => x.Description).Aggregate((x, y) => x + " " + y));
            return Result.Fail(DomainServiceErrors.InvalidPassword);
        }

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(
        string email,
        string newPassword,
        string otpCode)
    {
        var identityUser = await userManager.FindByEmailAsync(email);

        if (identityUser is null)
        {
            return DomainServiceErrors.UserNotFound;
        }

        var result = await userManager.ResetPasswordAsync(identityUser, otpCode, newPassword);

        if (result.Succeeded)
        {
            return Result.Success();
        }

        var error = result.Errors.First().Description;

        return Result.Fail(error);
    }

    public async Task<Result> RegisterAsTutor(
        CustomerId customerId,
        AcademicLevel academicLevel,
        string university,
        List<int> majors,
        List<string> verificationInfoDtos,
        CancellationToken cancellationToken = default)
    {
        // Check if the user existed
        var user = await userManager.FindByIdAsync(customerId.Value.ToString());
        var customer = await appDbContext.Customers.FindAsync(customerId, cancellationToken);

        if (user is null || customer is null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        // Check if the user is already a tutor
        var alTutor = await appDbContext.Tutors.FirstOrDefaultAsync(
            x => x.CustomerId == customer.Id,
            cancellationToken: default);

        if (alTutor is not null)
        {
            return Result.Fail(DomainServiceErrors.AlreadyTutorError);
        }

        var result = await userManager.AddToRoleAsync(user, Role.Tutor.ToString());

        if (!result.Succeeded)
        {
            logger.LogError("Fail to add role to user with id {userId} {Message}", customer.Id,
                result.Errors.Select(x => x.Description).Aggregate((x, y) => x + " " + y));

            return Result.Fail(DomainServiceErrors.FailRegisteringAsTutorErrorWhileAddingRole);
        }

        customer.RegisterAsTutor(majors, academicLevel,
            university,
            verificationInfoDtos);

        return Result.Success();
    }

    public async Task<Result> RemoveAsync(CustomerId customerId)
    {
        var user = await userManager.FindByIdAsync(customerId.ToString());

        if (user == null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        var result = await userManager.DeleteAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Fail(DomainServiceErrors.RemoveUserFail);
    }

    public async Task<Result> ForgetPasswordAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Fail(DomainServiceErrors.EmailNotConfirmed);
        }

        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // var callbackUrl = Url.Page(
        //     "/Account/ResetPassword",
        //     pageHandler: null,
        //     values: new { area = "Identity", code },
        //     protocol: Request.Scheme);

        // await emailSender.SendEmail(
        //     email,
        //     "Reset Password",
        //     $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        await emailSender.SendEmail(
            email,
            "Reset Password but just a demo",
            $"Please reset your password by this mail by using this {code}.");

        return Result.Success();
    }

    private EsIdentityUser InitializeUserInstance()
    {
        try
        {
            return Activator.CreateInstance<EsIdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(EsIdentityUser)}'. " +
                                                $"Ensure that '{nameof(EsIdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}