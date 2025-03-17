using System.Security.Cryptography;
using System.Text;

namespace CheckSUm.Helper
{
    public class SHAHelper
    {
        public async Task<string> UniqueNumGenarator(string number) {
            using (SHA256 sha256 = SHA256.Create()) { 
             byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(number));
             long numerichash = Math.Abs((long)BitConverter.ToUInt64(hash, 0));
                return (numerichash % 1000000).ToString("D6");
        }
        }
    }
}
