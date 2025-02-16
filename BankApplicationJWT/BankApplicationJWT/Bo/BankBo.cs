using BankApplicationJWT.Eception;
using BankApplicationJWT.Entity;
using BankApplicationJWT.Repo;
using Microsoft.Identity.Client;

namespace BankApplicationJWT.Bo
{
    public class BankBo
    {
        private readonly BankRepo bankRepo;

        public BankBo(BankRepo bankRepo)
        {
            this.bankRepo = bankRepo;
        }

        public string addAccount(Bank bank)
        {
            validateAmount(bank.Amount);
            validateAccount(bank.AccountNumber,bank.UserId);
            bool flag = bankRepo.addAccount(bank);
            return (flag == true) ? "Successfuly Account Created" : "Account Creation is Failed!!!";
        }

        public  Bank findByUserId(int userId)
        {
            var account = bankRepo.FindByuserId(userId);
            if (account == null) {
                throw new BankException("Account Not Exit for UserId :" + userId);
            }
            return account;
        }

        public decimal depositAmount(string accountNumber, decimal amount)
        {
            validateAccountNumber(accountNumber);
            validateAmount(amount);
            var newBalance = bankRepo.depositAmount(accountNumber, amount);
            return newBalance;
        }
        public decimal withdrawAmount(string accountNumber, decimal amount)
        {
            validateAccountNumber(accountNumber);
            validateAmount(amount);
            validateSufficientBalance(accountNumber, amount);
            var balance = bankRepo.withdrawAmount(accountNumber, amount);
            return balance;
        }

        public decimal transferAmount(string senderAccountNumber, string recipientAccountNumber, decimal amount)
        {
            validateAccountNumber(senderAccountNumber);
            validateAccountNumber(recipientAccountNumber);
            validateSufficientBalance(senderAccountNumber,amount);
            var balance = bankRepo.transferAmount(senderAccountNumber, recipientAccountNumber, amount);
            return balance;
        }
        //Validation
        private void validateAmount(decimal amount)
        {
            if (amount < 0) {
                throw new BankException("Amount Must be greater than Zero!!!");
            }
        }

        private void validateAccountNumber(string accountNumber)
        {
            bool flag = bankRepo.FindByAccountNo(accountNumber);
            if (!flag) {

                throw new BankException("Account Not Found in the DataBase!!!");
            }
        }

        private void validateAccount(string accountNumber,int userId)
        {
           bool flag = bankRepo.FindByAccountNo(accountNumber);
            bool userExit = bankRepo.FindByUserId(userId);
            if (flag) {

                throw new BankException("Account No Already Exit in dataBase!!!");
                   }
            if (userExit) {
                throw new UserException("User Already Exit in the Bank Data Base!!");
            }

        }

        private void validateSufficientBalance(string accountNumber, decimal amount)
        {
           Bank account = bankRepo.FindByAccountNum(accountNumber);
            if (account.Amount < amount) {
                throw new BankException("In sufficient Balance !!!");
            }
        }
    }
}
