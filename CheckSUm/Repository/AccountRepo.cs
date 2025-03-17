using CheckSUm.Data;
using CheckSUm.Entity;
using Microsoft.EntityFrameworkCore;

namespace CheckSUm.Repository
{
    public class AccountRepo
    {
        private readonly AppDbContext context;
        public AccountRepo(AppDbContext context)
        {
            this.context = context;
        }

        //Add Account
        public async Task<AccountDetails> AddAccount(AccountDetails accountDetails) {
            Console.Write($"{accountDetails.name}{accountDetails.accountNumber}");
            await context.Accounts.AddAsync(accountDetails);
            await context.SaveChangesAsync();
            return accountDetails;
        }

        //Find Acccount By Id
        public async Task<AccountDetails> FindAccountById(int id)
        {
          return await context.Accounts.FindAsync(id);
        }

        public async Task<AccountDetails> FindByAccountNumber(string accountNumber)
        {
            AccountDetails details =await context.Accounts.FirstOrDefaultAsync(a => a.accountNumber == accountNumber);
            if (details == null) {
                Console.WriteLine($"❌ Account not found for Account Number: {accountNumber}");
                return null; 
            }
            Console.WriteLine(details.accountNumber);
            return details;
        }

    }
}
