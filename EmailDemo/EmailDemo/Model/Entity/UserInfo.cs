using System.ComponentModel.DataAnnotations;

namespace EmailDemo.Model.Entity
{
    public class UserInfo
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string accountNumber { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
    }
}
