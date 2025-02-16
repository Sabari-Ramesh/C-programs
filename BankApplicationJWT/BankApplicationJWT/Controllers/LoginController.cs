using BankApplicationJWT.Bo;
using BankApplicationJWT.Data;
using BankApplicationJWT.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankApplicationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly UserBo _userBo;

        public LoginController(AppDbContext context, IConfiguration configuration, UserBo userBo)
        {
            _jwtKey = configuration["Jwt:Key"]!;
            _jwtIssuer = configuration["Jwt:Issuer"]!;
            _userBo = userBo;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                string resmsg = _userBo.addUser(user);
                return Ok(resmsg);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {

                User resUser = _userBo.Login(request);
                if (resUser == null)
                    return Unauthorized("Invalid credentials.");

                var token = GenerateJwtToken(resUser);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Method TO Generate the TOken Based on individual User :

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginRequest
        {
            public string UserEmail { get; set; }
            public string UserPassword { get; set; }
        }

    }
}
