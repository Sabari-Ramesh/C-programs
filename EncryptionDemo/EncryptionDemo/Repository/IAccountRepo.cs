using EncryptionDemo.Entity;

namespace EncryptionDemo.Repository
{
    public interface IAccountRepo
    {
       public  Task AddAccount(String name, String AccountNumber);
       public  Task<AccountDetails> FetchAccountNumber(int id);
    }
}
