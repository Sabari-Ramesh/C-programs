using System.Security.Cryptography;
using System.Text;

namespace EncryptionDemo.Helper
{
    public class AESEncryption
    {
        private static readonly string Key = Environment.GetEnvironmentVariable("AES_KEY") ?? "YourSecretKey123"; // 16, 24, or 32 characters
        private static readonly string IV = Environment.GetEnvironmentVariable("AES_IV") ?? "1234567890123456";   // Exactly 16 characters

        public string Encrypt(string plainText)
        {
            // Validate Key length
            if (Key.Length != 16 && Key.Length != 24 && Key.Length != 32)
            {
                throw new ArgumentException("AES Key must be 16, 24, or 32 characters long.");
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);

                // Generate a random IV
                aesAlg.GenerateIV();
                byte[] iv = aesAlg.IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Write the IV first (so it can be retrieved during decryption)
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    // Return the combined IV + ciphertext as a Base64 string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            // Validate Key length
            if (Key.Length != 16 && Key.Length != 24 && Key.Length != 32)
            {
                throw new ArgumentException("AES Key must be 16, 24, or 32 characters long.");
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);

                // Convert the Base64 string back to bytes
                byte[] combinedData = Convert.FromBase64String(cipherText);

                // Extract the IV (first 16 bytes)
                byte[] iv = new byte[16];
                Array.Copy(combinedData, iv, 16);

                // Extract the actual ciphertext (everything after the IV)
                byte[] cipherBytes = new byte[combinedData.Length - 16];
                Array.Copy(combinedData, 16, cipherBytes, 0, cipherBytes.Length);

                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

    }

}
