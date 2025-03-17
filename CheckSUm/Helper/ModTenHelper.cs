using System.Numerics; 

namespace CheckSUm.Helper
{
    public class ModTenHelper
    {

        //Add ModTenAccount
        public async Task<string> AddModTen(string accountNumber) {       
            BigInteger num = BigInteger.Parse(accountNumber);

            int len = accountNumber.Length-1;
            bool flag =false;
            long sum = 0;
            for (int i = accountNumber.Length - 1; i >= 0; i--) {
                int digit = accountNumber[i] - '0';
                if (flag) {
                    digit *= 2;
                    if (digit > 9) {
                        digit = (digit % 10) + (digit / 10);
                    }
                }
                sum += digit;
                flag = !flag;
            }
            long checkDigit = (10 - (sum % 10)) % 10;
           // return $"{accountNumber}|{checkDigit}";
           return $"{accountNumber}{checkDigit}";
        }


        //Return Account Number
        public async Task<string> RetriveAccountNum(String accountNumber) {
            string[] numbers = accountNumber.Split("|");
            string accNumber =numbers[0];
            string checkSum = numbers[1];
            string backendAccountNum =await AddModTen(accNumber);
            if (backendAccountNum == accountNumber) {
                return numbers[0];
            }
            return "Account Number is corrupted !!!";
        }

        //ValidateAccount
        public async Task<bool> ValidateAccountNumber(string UserAccountNumber, string BackendAccoutNumber) {
            string ModTenAccountNumber =await AddModTen(UserAccountNumber);
            return ModTenAccountNumber.Equals(BackendAccoutNumber);
        }
    }
}
