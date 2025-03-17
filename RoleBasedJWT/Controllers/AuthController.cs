using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleBasedJWT.Helper;
using System.Security.Claims;

namespace RoleBasedJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly JwtHelper _jwtHelper;

        public AuthController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        // Endpoint to generate a token and set it in a cookie
        [HttpPost("generate-token")]
        public IActionResult GenerateToken([FromBody] UserRequest request)
        {
           var token =  _jwtHelper.GenerateToken(request.UserId, request.role);
            Console.WriteLine($"{token} :Generated");
            _jwtHelper.SetTokenInCookie(Response, token);
            return Ok(new { Message = "Token generated and set in cookie." });
        }

        // Endpoint to validate the token from the cookie
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            // Extract Token from Cookie
            var token = Request.Cookies["jwt"];
            Console.WriteLine($"Validate :{token}");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Token not found in cookie." });
            }
            Console.WriteLine($"{token} :Validate");

            var principal = _jwtHelper.ValidateToken(token);

            if (principal == null)
            {
                return Unauthorized(new { Message = "Invalid or expired token." });
            }
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var roleClaim = principal.FindFirst(ClaimTypes.Role);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"ROle :{role}");
            Console.WriteLine(roleClaim);

            return Ok(new { Message = "Token is valid." });
        }

        //Admin Role Based Authetication

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndpoint()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine($"User ID: {userId}, Role: {role}");

            return Ok("Welcome, Admin! You have access.");
        }


        //User Role Based Authetication
        [HttpGet("user-only")]
        [Authorize(Roles = "User")]
        public IActionResult UserEndpoint()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine($"User ID: {userId}, Role: {role}");

            return Ok("Welcome, User! You have access.");
        }
    }


        // Model for User Request
        public class UserRequest
        {
            public string UserId { get; set; }
           public string role { get; set; }
        }


    }
