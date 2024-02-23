using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Users;

public class User : FullAuditedAggregateRoot<IdentityGuid>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Gender Gender { get; private set; } = Gender.Male;
    public int BirthYear { get; private set; } = 1990;
    public Address Address { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Avatar { get; private set; } = @"default_avatar";
    public string Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public UserRole Role { get; private set; } = UserRole.Learner;

    private User()
    {
    }

    internal static User Create(IdentityGuid identityUserId)
    {
        return new User
        {
            Id = identityUserId
        };
    }

    internal static User Create(
        IdentityGuid identityUserId,
        string firstName,
        string lastName,
        Gender gender,
        int birthYear,
        Address address,
        string description,
        string avatar,
        string email,
        string phoneNumber,
        UserRole role)
    {
        return new User
        {
            Id = identityUserId,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            BirthYear = birthYear,
            Address = address,
            Description = description,
            Avatar = avatar,
            Email = email,
            PhoneNumber = phoneNumber,
            Role = role
        };
    }

    public string GetFullName()
    {
        return FirstName + " " + LastName;
    }

    public void SetAvatar(string result)
    {
        Avatar = result;
    }
}