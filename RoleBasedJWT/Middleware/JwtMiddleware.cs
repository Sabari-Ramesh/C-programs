
/*
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoleBasedJWT.Middleware
{
    public class JwtMiddleware
    {
                private readonly RequestDelegate _next;
                private readonly string _secret;
                private readonly string _issuer;
                private readonly string _audience;

                public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
                {
                    _next = next;

                    // Load JWT settings from configuration
                    var jwtSettings = configuration.GetSection("Jwt");
                    _secret = jwtSettings["Secret"];
                    _issuer = jwtSettings["Issuer"];
                    _audience = jwtSettings["Audience"];
                }

                    public async Task Invoke(HttpContext context)
                    {
                        if (context.Request.Cookies.TryGetValue("jwt", out var token))
                        {
                            try
                            {
                                var tokenHandler = new JwtSecurityTokenHandler();
                                var key = Encoding.UTF8.GetBytes(_secret);

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
                                    NameClaimType = ClaimTypes.Name,
                                    RoleClaimType = ClaimTypes.Role,                                    
                                }, out SecurityToken validatedToken);

                                context.User = principal;
                    //  context.Request.Headers.Append("Authorization", $"Bearer {token}");
                             }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                await context.Response.WriteAsJsonAsync(new { Message = "Invalid token.", Error = ex.Message });
                                return;
                            }
                        }

                        await _next(context);
                    }
    }
}


*/



//=====================================================================

/* private readonly RequestDelegate _next;

         public JwtMiddleware(RequestDelegate next)
         {
             _next = next;
         }

         public async Task Invoke(HttpContext context)
         {
             // Check if the "jwt" cookie exists
             if (context.Request.Cookies.TryGetValue("jwt", out var token))
             {
                 context.Request.Headers.Append("Authorization", $"Bearer {token}");
             }
             await _next(context);
         }

       */
///UnAuthorized Not Send
