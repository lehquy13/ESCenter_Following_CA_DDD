using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ESCenter.Application.Contracts.Authentications;
using ESCenter.Application.Interfaces.Authentications;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

internal class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = options.Value;

    public string GenerateToken(UserLoginDto userLoginDto)
    {
        var signingCredential = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256
        );
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userLoginDto.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userLoginDto.FullName),
            new Claim(JwtRegisteredClaimNames.Email, userLoginDto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, userLoginDto.Role)
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredential
        );
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public bool ValidateToken(string token)
    {
        string accessToken = token;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                // ClockSkew = TimeSpan.Zero // zero tolerance for the token lifetime expiration time
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                // token is expired, redirect to authentication page
                return false;
            }
            
            // token is still valid, navigate to home page
            return true;
        }
        catch
        {
            return false;
        }
    }
}