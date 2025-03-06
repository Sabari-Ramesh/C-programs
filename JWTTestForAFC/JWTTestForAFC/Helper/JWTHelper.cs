using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;

    public JwtHelper(string secretKey, string issuer, string audience, int accessTokenExpirationMinutes)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
        _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
    }

    // Generate JWT Token
    public string GenerateToken(int userId, string userName, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Validate JWT Token
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return principal;
        }
        catch
        {
            return null; // Token is invalid
        }
    }

    // Create Cookie with JWT Token
    public void CreateCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevent client-side JavaScript access
            Secure = true,   // Ensure cookie is sent over HTTPS
            SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
            Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes) // Match token expiration
        };

        response.Cookies.Append("jwtToken", token, cookieOptions);
    }

    // Clear Cookie (Logout)
    public void ClearCookie(HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1) // Expire immediately
        };

        response.Cookies.Append("jwtToken", "", cookieOptions); // Empty value to clear the cookie
    }
}