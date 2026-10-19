using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Astibosoft.Api.Domain.Admins;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Astibosoft.Api.Infrastructure.Auth;

public class JwtTokenIssuer(IOptions<JwtOptions> options)
{
    public string CreateAdminToken(Admin admin)
    {
        var jwt = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, admin.Email),
            new(ClaimTypes.Name, admin.Name),
            new(ClaimTypes.Role, "Admin"),
        };

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
