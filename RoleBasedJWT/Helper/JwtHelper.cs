using Microsoft.IdentityModel.Tokens;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Model.Entity;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace RoleBasedJWT.Helper
{
    public class JwtHelper
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireTimeInHours;
        private readonly string _secret;

        public JwtHelper(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            _issuer = jwtSettings["Issuer"];
            _audience = jwtSettings["Audience"];
            _expireTimeInHours = int.Parse(jwtSettings["ExpireTimeInHours"]);
            _secret = jwtSettings["Secret"];
        }

        // Generate JWT Token
        public string GenerateToken(string userId,string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Console.WriteLine($"Generate Token Role :{role}");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Name, userId), // Maps to ClaimsPrincipal.Identity.Name
            new Claim("UserId", userId),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_expireTimeInHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Set JWT Token in Cookie
        public void SetTokenInCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(_expireTimeInHours),
                Secure = true, 
                SameSite = SameSiteMode.Strict
            };

            response.Cookies.Append("jwt", token, cookieOptions);
        }

        // Validate JWT Token
        public ClaimsPrincipal ValidateToken(string rawToken)
        {
            if (string.IsNullOrEmpty(rawToken))
            {
                return null; 
            }

            // Clean up the token (remove extra attributes and trim whitespace)
            var token = rawToken.Split(';')[0].Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try
            {
                // Validate the token
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name,
                }, out SecurityToken validatedToken);
                var userId = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                Console.WriteLine($"Extracted UserId: {userId}");
                var roleClaim = principal.FindFirst(ClaimTypes.Role);
                Console.WriteLine($"Role Claim: {roleClaim?.Value}");
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null; // Invalid token
            }
        }


    }
}




































/*       public string GenerateToken(int userId, string role) //, string role
       {
           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
           var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"]));

           var claims = new[]
           {
           new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
          new Claim(ClaimTypes.Role, role),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expires).ToUnixTimeSeconds().ToString())
       };

           var token = new JwtSecurityToken(
               _configuration["Jwt:Issuer"],
               _configuration["Jwt:Audience"],
               claims,
               expires: expires,
               signingCredentials: credentials
           );

           return new JwtSecurityTokenHandler().WriteToken(token);
       } */


/*     public ClaimsPrincipal ValidateToken(string token)
     {
         var tokenHandler = new JwtSecurityTokenHandler();
         var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

         var parameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(key),
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidIssuer = _configuration["Jwt:Issuer"],
             ValidAudience = _configuration["Jwt:Audience"],
             ValidateLifetime = true,  // Ensures expired tokens are rejected
             ClockSkew = TimeSpan.Zero // No extra time allowance
         };

         return tokenHandler.ValidateToken(token, parameters, out _);
     }  */