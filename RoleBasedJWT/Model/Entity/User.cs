
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleBasedJWT.Model.Entity
{
    public class User
    {
        [Key] // Primary Key
        public int UserId { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Email { get;  set; }

        [Required]
        public string Password { get; set; }

        // Navigation property for related accounts (One-to-Many Relationship)
        public ICollection<Account> Accounts{ get; set; }

        }
}
