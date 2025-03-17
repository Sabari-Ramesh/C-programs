using EncryptionDemo.Data;
using EncryptionDemo.Entity;

namespace EncryptionDemo.Repository
{
    public class AccountRepo : IAccountRepo
    {

        private readonly AppDbContext context;

        public AccountRepo(AppDbContext context)
        {
            this.context = context;
        }

        //Add Account
        public async Task AddAccount(string name, string accountNumber)
        {
            AccountDetails details = new AccountDetails()
            {
                name = name,
                accountNumber = accountNumber
            };
            Console.Write($"{name}{accountNumber}");
            await context.Accounts.AddAsync(details);
           await context.SaveChangesAsync();
        }


        //Find Account By Id
        public async Task<AccountDetails> FetchAccountNumber(int id)
        {
           return await context.Accounts.FindAsync(id);
        }
    }
}
