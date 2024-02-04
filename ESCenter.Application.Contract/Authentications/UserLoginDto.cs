namespace ESCenter.Application.Contracts.Authentications;

public record UserLoginDto
{
    public int Id { get; set; }

    //User information
    public string FullName { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    //Account References
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}

