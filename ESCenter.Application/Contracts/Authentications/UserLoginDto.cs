namespace ESCenter.Application.Contracts.Authentications;

public class UserLoginDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}

