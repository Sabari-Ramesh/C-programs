using System.ComponentModel.DataAnnotations;

namespace EmailDemo.Model
{
    public class User
    {
        public string userName { get; set; }
        public string userEmail { get; set; }

        public string subject { get; set; }

        public string body { get; set; }
        public string city { get; set; }
    }
}
