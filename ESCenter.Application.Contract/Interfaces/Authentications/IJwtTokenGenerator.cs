//using EduSmart.Application.Users;

using ESCenter.Application.Contracts.Authentications;

namespace ESCenter.Application.Contracts.Interfaces.Authentications
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserLoginDto userLoginDto);
        bool ValidateToken(string token);
    }
}
