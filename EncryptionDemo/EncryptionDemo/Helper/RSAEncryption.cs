using System.Security.Cryptography;
using System.Text;

namespace EncryptionDemo.Helper
{
    public class RSAEncryption
    {
        private readonly string publicKey = "Keys/publicKey.xml";
        private readonly string privateKey = "Keys/privateKey.xml";

        //Constructor
        public RSAEncryption(IConfiguration configuration)
        {
            GenerateKeys();
        }

        //Generate Keys
        private void GenerateKeys()
        {
            if (!File.Exists(publicKey) || !File.Exists(privateKey))
            {
                using (RSA rsa = RSA.Create(2048))
                {
                    Directory.CreateDirectory("Keys");
                    File.WriteAllText(publicKey, rsa.ToXmlString(false)); //This method Contains the public Key file Path  false => public Key Saved in one File
                    File.WriteAllText(privateKey, rsa.ToXmlString(true)); //This method Contains the private Key file Path True =>Private Key Saved in one File
                }
            }
        }

        //Encryption
        public string Encryption(String plaintext) {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(File.ReadAllText(publicKey));  //Read the public Key from the File
                string timestamp = DateTime.UtcNow.Ticks.ToString(); // Unique per request
                string dataWithTime = timestamp + "|" + plaintext;
                byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(dataWithTime), RSAEncryptionPadding.OaepSHA256); //First Convert the plain text into the Bytes and then add the Padding to it Multiples of 16
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        //Decryption
        public string Decryption(string cipherText)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(File.ReadAllText(privateKey));
                byte[] decryptedBytes = rsa.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.OaepSHA256); //First Convert the plain text into the Bytes and then add the Padding to it Multiples of 16                                                                                                                   //return Encoding.UTF8.GetString(decryptedBytes);
                string decryptedString = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedString.Substring(decryptedString.IndexOf("|") + 1);
            }
        }
    }
}
