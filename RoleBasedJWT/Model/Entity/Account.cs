using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleBasedJWT.Model.Entity
{
    public class Account
    {
        [Key] 
        public int AccountNo { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key

        // Navigation property for User (Many-to-One Relationship)
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public string BankName { get; set; }
    }
}
