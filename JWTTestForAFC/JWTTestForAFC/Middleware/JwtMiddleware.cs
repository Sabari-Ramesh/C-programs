using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtHelper _jwtHelper;

    public JwtMiddleware(RequestDelegate next, JwtHelper jwtHelper)
    {
        _next = next;
        _jwtHelper = jwtHelper;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["jwtToken"] ?? context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            var principal = _jwtHelper.ValidateToken(token);

            if (principal != null)
            {
                context.User = principal; // Attach the user principal to the context
            }
        }

        await _next(context);
    }
}