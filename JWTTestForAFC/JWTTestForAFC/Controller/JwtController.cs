using JWTTestForAFC.Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtHelper _jwtHelper;

    public AuthController(JwtHelper jwtHelper)
    {
        _jwtHelper = jwtHelper;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        // Authenticate user (e.g., check username and password against database)
        if (user.UserName == "test" && user.Password == "password")
        {
            // Generate JWT Token
            var token = _jwtHelper.GenerateToken(user.UserId, user.UserName, user.Role);

            // Set JWT Token in Cookie
            _jwtHelper.CreateCookie(Response, token);

            return Ok(new { Message = "Login successful", Token = token });
        }

        return Unauthorized("Invalid credentials");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Clear JWT Token from Cookie
        _jwtHelper.ClearCookie(Response);

        return Ok(new { Message = "Logout successful" });
    }
}