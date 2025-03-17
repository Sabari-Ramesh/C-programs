using CheckSUm.Entity;
using CheckSUm.Helper;
using CheckSUm.Repository;
using System.Reflection.PortableExecutable;

namespace CheckSUm.Service
{
    public class AccountService
    {
        private readonly AccountRepo repo;
        private readonly CheckSum checkSumHelper;
        private readonly ModTenHelper modTenHelper;
        private readonly SHAHelper shaHelper;

        //Constructor
        public AccountService(AccountRepo repo , CheckSum checkSum,ModTenHelper modTenHelper,SHAHelper shaHelper) { 
          this.repo = repo;
          checkSumHelper = checkSum;
          this.modTenHelper = modTenHelper;
          this.shaHelper = shaHelper;
        }

        //Add Account In DataBase  //Bottom Up Approach
        public async Task<AccountDetails> AccountDetails(AccountDetails details) {
        string accountNumber = checkSumHelper.Sender(details.accountNumber);
        details.accountNumber = accountNumber;
        return await repo.AddAccount(details);
        }

        //Find Acccount By Id //Bottom Up Approach
        public async Task<AccountDetails> FindAccountById(int id)
        {
            AccountDetails details = await repo.FindAccountById(id);
            String accountNumber = checkSumHelper.Validate(details.accountNumber);
            details.accountNumber = accountNumber;
            return details;
        }


        //ModTenApproach
        public async Task<AccountDetails> ModTenAddAccountNumber(AccountDetails details) { 
            string accountNumber =await modTenHelper.AddModTen(details.accountNumber);
            details.accountNumber = accountNumber;
            return await repo.AddAccount(details);
        }

        //Find Account By Id // MOdTen Appproach
        public async Task<AccountDetails> ModTenFindAccountById(int id) { 
            AccountDetails details = await repo.FindAccountById(id);
            string accountNumber = await modTenHelper.RetriveAccountNum(details.accountNumber);
            details.accountNumber= accountNumber;
            return details;
        }

        //Add Random Generated AccountNUmber Between Fixed AccountNumber
        public async Task<string> RandomAccountNumber() {
            DateTime timeStamp = DateTime.UtcNow;
            TimeZoneInfo easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeStamp, easternTimeZone);
            string str = easternTime.ToString("o");
            Console.WriteLine(str);
            string resStr = "";
            for (int i = 0; i < str.Length; i++) { 
                char ch = str[i];
                if (char.IsDigit(ch)) { 
                resStr += ch; 
                }
            }
            Console.WriteLine($"Time Zone :{resStr}");
            return await shaHelper.UniqueNumGenarator(resStr);
        }

        //Random Number Generator as per Requirement.
        public async Task<AccountDetails> AccountNumberRequirements(string sector, string currencyNum)
        {
            /*  Random random = new Random(); 
              string ranNum =random.Next(1,1000000).ToString("D6");
              Console.WriteLine(ranNum);
            */
            string ranNum = await RandomAccountNumber();
            string modTenCheckSum =await modTenHelper.AddModTen(ranNum);
            Console.WriteLine("modTen "+modTenCheckSum);
            string fixedNum = "7";
            string accountNumber = "7"+modTenCheckSum+sector+currencyNum;
            Console.WriteLine($"{accountNumber} : Random Number {ranNum}");
            AccountDetails details = new AccountDetails()
            {
                name = "Sabari",
                accountNumber = accountNumber,
            };
            return await repo.AddAccount(details);
        }

        public async Task<string> ValidateAccountNumber(string accountNumber)
        {
           // string checkModTen = accountNumber.Substring(1, 7);
            string subStr = await modTenHelper.AddModTen(accountNumber.Substring(1, 6));
            Console.WriteLine($"Given AccoutNum :{subStr} AccountNumber : {accountNumber.Substring(1, 8)}");

            if (!subStr.Equals(accountNumber.Substring(1, 7))) {
                return "Mod 10 Validation is Failed!!!";
               // return false;
            }
            AccountDetails details = await repo.FindByAccountNumber(accountNumber);
            if (details == null)
            {
                Console.WriteLine("Account not found in database.");
                return "Account does Not exit in DataBase!!!";
            }
            string fetchAccountNumber = details.accountNumber;
            if (!fetchAccountNumber.Equals(accountNumber)) {
                return "InValid Account Number";
            }
            return "Correct Account Number !!!!";
        }
    }
}
