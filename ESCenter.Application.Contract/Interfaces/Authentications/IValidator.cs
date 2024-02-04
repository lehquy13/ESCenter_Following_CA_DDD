namespace ESCenter.Application.Contracts.Interfaces.Authentications;

public interface IValidator
{
    public string GenerateValidationCode();
    public string HashPassword(string input);

}