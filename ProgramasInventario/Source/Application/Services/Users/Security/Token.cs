using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Users.Security;

public static class Token
{
    public static string GenerateJwt(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Audience = GlobalConfigurations.JwtAudience,
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = GlobalConfigurations.JwtIssuer,
            SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new[] { new Claim(nameof(email), email) })
        });

        return tokenHandler.WriteToken(token);
    }

    public static string GetClaimFrom(StringValues authorization)
    {
        var token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(authorization.ToString().Trim()[7..]);

        return token.Claims.First(claim => claim.Type.ToLowerInvariant() == "email").Value;
    }

    public static SymmetricSecurityKey GetSecurityKey() =>
        new(Encoding.ASCII.GetBytes(GlobalConfigurations.JwtSecurityKey));
}