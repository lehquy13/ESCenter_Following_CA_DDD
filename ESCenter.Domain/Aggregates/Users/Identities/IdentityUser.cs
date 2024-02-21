using System.Security.Cryptography;
using System.Text;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.SharedKernel.Domain.Primitives;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Users.Identities;

public class IdentityUser : FullAuditedAggregateRoot<IdentityGuid>
{
    private IdentityUser()
    {
    }

    public static IdentityUser Create(
        string? userName,
        string? email,
        string? password,
        string? phoneNumber,
        IdentityRoleId identityRoleId)
    {
        var identityUser = new IdentityUser
        {
            Id = IdentityGuid.Create()
        };
        identityUser.SetUserName(userName);
        identityUser.SetEmail(email);
        identityUser.HandlePassword(password);
        identityUser.IdentityRoleId = identityRoleId;
        identityUser.PhoneNumber = phoneNumber;

        return identityUser;
    }
    
    public static IdentityUser Create(
        string? userName,
        string? email,
        string? password,
        string? phoneNumber,
        IdentityGuid identityId,
        IdentityRoleId identityRoleId)
    {
        var identityUser = new IdentityUser
        {
            Id = identityId
        };
        identityUser.SetUserName(userName);
        identityUser.SetEmail(email);
        identityUser.HandlePassword(password);
        identityUser.IdentityRoleId = identityRoleId;
        identityUser.PhoneNumber = phoneNumber;

        return identityUser;
    }

    public string? UserName { get; private set; }

    public string? NormalizedUserName { get; private set; }

    public string? Email { get; private set; }

    public string? NormalizedEmail { get; private set; }

    public bool EmailConfirmed { get; private set; }

    public byte[]? PasswordHash { get; private set; }

    public byte[]? PasswordSalt { get; private set; }

    public string? ConcurrencyStamp { get; private set; } = Guid.NewGuid().ToString();

    public string? PhoneNumber { get; private set; }

    public bool PhoneNumberConfirmed { get; private set; }

    public OtpCode OtpCode { get; private set; } = new();

    public IdentityRoleId IdentityRoleId { get; private set; } = null!;

    public IdentityRole IdentityRole { get; private set; } = null!;

    //public User User { get; private set; } = null!;

    //public virtual DateTimeOffprivate set? LockoutEnd { get; private set; }

    //public virtual bool LockoutEnabled { get; private set; }

    //public virtual int AccessFailedCount { get; private set; }

    internal void SetUserName(string? value)
    {
        if (UserName is not null && value is null) throw new ArgumentException("Invalid user name.");

        if (value is not null) NormalizedUserName = value.ToUpperInvariant();

        UserName = value;
    }

    internal void SetEmail(string? value)
    {
        if (Email is not null && value is null) throw new ArgumentException("Invalid email.");

        if (value is not null) NormalizedEmail = value.ToUpperInvariant();

        Email = value;
    }

    internal void HandlePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Invalid password.");

        using var hmac = new HMACSHA256();

        PasswordSalt = hmac.Key;
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool ValidatePassword(string password)
    {
        if (PasswordHash is null || PasswordSalt is null) throw new ArgumentException("Invalid password.");

        using var hmac = new HMACSHA256(PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (var i = 0; i < computedHash.Length; i++)
            if (computedHash[i] != PasswordHash[i])
                return false;

        return true;
    }

    public override string ToString()
    {
        return UserName ?? string.Empty;
    }

    public void GenerateOtpCode()
    {
        OtpCode.Reset();
    }
    
    public void SetRole(IdentityRole role)
    {
        IdentityRole = role;
    }
}