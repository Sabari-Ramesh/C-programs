namespace EmailDemo.Helper
{
    public class EmailSettings
    {
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
    }
}
