using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckSUm.Entity
{
    public class AccountDetails
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }
            [Required]
            public string name { get; set; }
            [Required]
            public string accountNumber { get; set; }
    }
}
