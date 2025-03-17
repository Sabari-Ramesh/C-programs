
using EncryptionDemo.Repository;

namespace EncryptionDemo.Service
{
    public class AccountService
    {
        private readonly IAccountRepo _accountRepo;

        public AccountService(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task AddAccount(string name, string accountNumber)
        {
           _accountRepo.AddAccount(name, accountNumber);
            Console.WriteLine(name);
        }
    }
}
