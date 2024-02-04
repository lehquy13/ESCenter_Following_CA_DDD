using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Users;

public class User : FullAuditedAggregateRoot<IdentityGuid>
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Gender { get; private set; } = GenderEnum.Male;
    public int BirthYear { get; private set; } = 1990;
    public Address Address { get; private set; } = new();
    public string Description { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = @"default_avatar";
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    private User()
    {
    }

    // TODO: Email and Phone number are not updated
    internal static User Create(IdentityGuid identityUserId)
    {
        return new User
        {
            Id = identityUserId
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