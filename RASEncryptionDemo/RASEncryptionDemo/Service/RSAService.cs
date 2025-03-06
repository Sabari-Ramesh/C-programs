using System.Security.Cryptography;
using System.Text;

namespace RASEncryptionDemo.Service
{
    public class RSAService
    {
        private readonly string publicKeyPath = "Keys/publicKey.xml";
        private readonly string privateKeyPath = "Keys/privateKey.xml";

        public RSAService()
        {
            if (!File.Exists(publicKeyPath) || !File.Exists(privateKeyPath))
            {
                GenerateKeys();
            }
        }

        /// <summary>
        /// Generates and saves RSA public/private key pair.
        /// </summary>
        private void GenerateKeys()
        {
            using (RSA rsa = RSA.Create(2048))
            {
                Directory.CreateDirectory("Keys");
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false)); // Public Key (No Private Info)
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true)); // Private Key
            }
        }

        /// <summary>
        /// Encrypts a message using the public key.
        /// </summary>
        public string Encrypt(string plaintext)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Decrypts a message using the private key.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                byte[] decryptedBytes = rsa.Decrypt(Convert.FromBase64String(encryptedText), RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
