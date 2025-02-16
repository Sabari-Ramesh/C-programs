using System.ComponentModel.DataAnnotations;

namespace BankApplicationJWT.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [MaxLength(10)]
        public string Password { get; set; }
        public Bank? Bank { get; set; }
    }
}
