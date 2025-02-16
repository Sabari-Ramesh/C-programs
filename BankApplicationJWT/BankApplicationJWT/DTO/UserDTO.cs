using System.ComponentModel.DataAnnotations;

namespace BankApplicationJWT.DTO
{
    public class UserDTO
    {

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
