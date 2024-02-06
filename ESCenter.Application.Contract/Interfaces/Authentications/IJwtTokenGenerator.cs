//using EduSmart.Application.Users;

using ESCenter.Application.Contract.Authentications;

namespace ESCenter.Application.Contract.Interfaces.Authentications
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserLoginDto userLoginDto);
        bool ValidateToken(string token);
    }
}
