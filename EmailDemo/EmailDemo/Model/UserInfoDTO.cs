using System.ComponentModel.DataAnnotations;

namespace EmailDemo.Model
{
    public class UserInfoDTO
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
