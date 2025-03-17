//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using RoleBasedJWT.Helper;
//using RoleBasedJWT.Model.Dto;
//using RoleBasedJWT.Service;

//namespace RoleBasedJWT.Controllers
//{
//    public class UserController:ControllerBase
//    {

//        private readonly UserService userService;
//        private readonly JwtHelper jwtHelper;

//        public UserController(UserService userService,JwtHelper helper) { 
//        this.userService = userService;
//        this.jwtHelper = helper;
//        }

//        //Add User
//        [HttpPost("/addUser")]
//        public async Task<IActionResult> AddUser([FromBody] UserDto user) {
//            if (user == null) {
//                return BadRequest("User Canot be Null");
//            }
//            UserDto userDto =await userService.AddUser(user);
//            var token = jwtHelper.GenerateToken(userDto.UserId,userDto.Role,Response);
//          //  jwtHelper.CreateCookie(Response,token);
//            Console.WriteLine($"{userDto.UserId}:{userDto.Role}");
//            return Ok(token);
//        }



//        //Validate
//        [Authorize] // Requires a valid JWT token
//        [HttpGet("secure")]
//        public IActionResult SecureEndpoint()
//        {
//            return Ok("This is a secure endpoint. You are authenticated.");
//        }

//        [Authorize(Roles = "Admin")] // Requires user to have Admin role
//        [HttpGet("admin-only")]
//        public IActionResult AdminEndpoint()
//        {
//            /*  var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); // Extract token from header
//              var userId = jwtHelper.ValidateToken(token);

//              if (userId == null)
//                  return Unauthorized("Invalid token or userId not found");

//              return Ok(new { UserId = userId }); */

//            var userId = jwtHelper.ValidateToken(Request);
//            if (userId == null || userId==-1)
//            {
//                return Unauthorized("Invalid or expired token.");
//            }

//            return Ok(new { userId });
//        }

//        [Authorize(Roles = "User")] // Requires user to have User role
//        [HttpGet("user-only")]
//        public IActionResult UserEndpoint()
//        {
//            return Ok("Welcome, User! You have access.");
//        }

//    }
//}

/*

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedJWT.Helper;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Service;

namespace RoleBasedJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly JwtHelper jwtHelper;

        public UserController(UserService userService, JwtHelper jwtHelper)
        {
            this.userService = userService;
            this.jwtHelper = jwtHelper;
        }

        // Add User
        [HttpPost("/addUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }

            // Add the user using the service
            UserDto userDto = await userService.AddUser(user);

            // Generate JWT token and create a cookie
            var (token, cookieOptions) = jwtHelper.GenerateTokenWithCookie(userDto.UserId.ToString(), userDto.Role);

            // Set the token in a cookie
            Response.Cookies.Append("jwtToken", token, cookieOptions);

            // Log the user details
            Console.WriteLine($"UserId: {userDto.UserId}, Role: {userDto.Role}");

            return Ok(new { Message = "User added successfully", Token = token });
        }

        // Secure Endpoint (Requires authentication)
        [Authorize] // Requires a valid JWT token
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("This is a secure endpoint. You are authenticated.");
        }

        // Admin-Only Endpoint (Requires Admin role)
        [Authorize(Roles = "Admin")] // Requires user to have Admin role
        [HttpGet("admin-only")]
        public IActionResult AdminEndpoint()
        {
            // Extract UserId from the validated token (automatically handled by middleware)
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid or expired token.");
            }

            return Ok(new { UserId = userId, Message = "Welcome, Admin! You have access." });
        }

        // User-Only Endpoint (Requires User role)
        [Authorize(Roles = "User")] // Requires user to have User role
        [HttpGet("user-only")]
        public IActionResult UserEndpoint()
        {
            // Extract UserId from the validated token (automatically handled by middleware)
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid or expired token.");
            }

            return Ok(new { UserId = userId, Message = "Welcome, User! You have access." });
        }
    }
}

*/