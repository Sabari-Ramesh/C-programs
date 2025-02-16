using BankApplicationJWT.Data;
using BankApplicationJWT.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationJWT.Repo
{
    public class BankRepo
    {
        private readonly AppDbContext context;

        public BankRepo(AppDbContext context)
        {
            this.context = context;
        }

        public bool addAccount(Bank bank)
        {
            context.Banks.Add(bank);
            int changes = context.SaveChanges();
            return changes > 0 ? true : false;
        }

        public Bank? FindByuserId(int userId)
        {
            return context.Banks.FirstOrDefault(b => b.UserId == userId);
        }
        public  decimal depositAmount(string accountNumber, decimal amount)
        {
            var account = context.Banks.FirstOrDefault(b => b.AccountNumber == accountNumber);
            account.Amount += amount;
            context.SaveChanges();
            return account.Amount;
        }
        public decimal withdrawAmount(string accountNumber, decimal amount)
        {
            var account = context.Banks.FirstOrDefault(b => b.AccountNumber == accountNumber);
            account.Amount -= amount;
            context.SaveChanges();
            return account.Amount;
        }
        public decimal transferAmount(string senderAccountNumber, string recipientAccountNumber, decimal amount)
        {
            var senderAccount = context.Banks.First(b => b.AccountNumber == senderAccountNumber);
            var recipientAccount = context.Banks.First(b => b.AccountNumber == recipientAccountNumber);
            senderAccount.Amount -= amount;
            recipientAccount.Amount += amount;
            context.SaveChanges();
            return senderAccount.Amount;
        }


        //validation Purpose
        public bool FindByAccountNo(string accountNumber)
        {
            return context.Banks.Any(u => u.AccountNumber == accountNumber);
        }

        public  bool FindByUserId(int userId)
        {
            return context.Banks.Any(u => u.UserId == userId);
        }

        public Bank FindByAccountNum(string accountNumber)
        {
            return context.Banks.FirstOrDefault(b => b.AccountNumber == accountNumber);
        }
    }
}
