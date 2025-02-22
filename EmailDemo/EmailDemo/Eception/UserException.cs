namespace EmailDemo.Eception
{
    public class UserException: Exception
    {
        public UserException(string message) : base(message) { }
    }
}
