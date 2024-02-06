namespace ESCenter.Application.Contract.Authentications;

public class AuthenticationResult
{
    public UserLoginDto User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}

