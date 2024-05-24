using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Utilities;

public static class AuthHelper
{
    private static string SecurityAlgorithm => SecurityAlgorithms.HmacSha256;

    public static string UpdateTokenExpiration(JwtSecurityToken token, DateTime newExpiration, string secret)
    {
        return GenerateAccessToken(token.Issuer, token.Audiences.First(), secret, token.Claims, newExpiration);
    }

    public static string GenerateAccessToken(string issuer, string audience, string secret,
        IEnumerable<Claim> claims, DateTime expiration)
    {
        var secretKey = GetSymmetricSecurityKey(secret);
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithm);

        var token = new JwtSecurityToken
        (
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Generates a 256-bit cryptographically secure random number and then converts it to a Base64 string
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32]; // 256 bits
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static IEnumerable<Claim> GetClaims(JwtAccessTokenClaimsInputDto item)
    {
        var claims = new List<Claim>
        {
            new("sub", item.UserId),
            new("name", item.UserDisplayName),
            new("username", item.UserName)
        };

        // Add user roles
        claims.AddRange(item.UserRoles.Select(role => new Claim("roles", role)));

        var isAdmin = item.UserRoles.Contains("Administrator");
        claims.Add(new Claim("isAdmin", isAdmin.ToString(), ClaimValueTypes.Boolean));

        return claims;
    }

    public static JwtSecurityToken? ValidateJwtToken(string token, string issuer, string audience, string secret,
        bool validateLifetime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = GetSymmetricSecurityKey(secret)
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
            var validatedToken = (JwtSecurityToken)validatedSecurityToken;
            return validatedToken;
        }
        catch
        {
            return null;
        }
    }

    private static SymmetricSecurityKey GetSymmetricSecurityKey(string secret)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }
}