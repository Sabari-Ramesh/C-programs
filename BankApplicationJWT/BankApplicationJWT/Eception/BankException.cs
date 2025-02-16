namespace BankApplicationJWT.Eception
{
    public class BankException:Exception
    {
        public BankException() { }
        public BankException(String message) : base(message) { }
    }
}
