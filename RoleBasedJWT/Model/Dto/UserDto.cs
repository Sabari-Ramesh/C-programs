using RoleBasedJWT.Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedJWT.Model.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
