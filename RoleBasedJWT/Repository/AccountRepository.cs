using RoleBasedJWT.Data;
using RoleBasedJWT.Model.Entity;

namespace RoleBasedJWT.Repository
{
    public class AccountRepository
    {
        private readonly AppDbContext context;
        public AccountRepository(AppDbContext context) {
        this.context = context;
        }

        //Add Account in DataBase
        public async Task<Account> AddAccount(Account account)
        {
            if (account == null) {
                return null;
            }
            //var user = await context.users.FindAsync(account.UserId);
            //account.UserId = user.UserId;
            await context.accounts.AddAsync(account);
            context.SaveChanges();
            return account;
        }
    }
}
