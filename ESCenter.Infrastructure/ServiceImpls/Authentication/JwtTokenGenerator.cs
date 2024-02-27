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
        var claims = new Claim[]
        {
            new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userLoginDto.Id.ToString()),
            new(ClaimTypes.Name, userLoginDto.FullName),
            new(ClaimTypes.Email, userLoginDto.Email),
            new(ClaimTypes.Role, userLoginDto.Role),
            //new(ClaimTypes.Actor, userLoginDto.TenantId.ToString())
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