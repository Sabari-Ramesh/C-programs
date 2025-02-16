using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationJWT.Entity
{
    public class Bank
    {
        [Key,MaxLength(10)]
        public String AccountNumber{ get; set; }
        [Required]
        public int UserId { get; set;}

        public decimal Amount { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}
