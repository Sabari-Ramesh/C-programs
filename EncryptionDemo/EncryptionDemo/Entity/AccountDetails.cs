using System.ComponentModel.DataAnnotations;

namespace EncryptionDemo.Entity
{
    public class AccountDetails
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string accountNumber { get; set; }
    }
}
