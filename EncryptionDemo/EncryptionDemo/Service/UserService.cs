using EncryptionDemo.Entity;
using EncryptionDemo.Helper;
using EncryptionDemo.Repository;

namespace EncryptionDemo.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly AESEncryption encryption;
        public UserService(IUserRepository userRepository, AESEncryption encryption)
        {
            this.userRepository = userRepository;
            this.encryption = encryption;
        }

        public async Task addUser(string userName,string userInformation) { 
            //Encrypt the Data
            String encryptedDataInfo = encryption.Encrypt(userInformation);
            Console.WriteLine(encryptedDataInfo);
            Console.WriteLine(userName);

            var user = new User
            {
                name = userName,
                encryptedData = encryptedDataInfo
            };
            await userRepository.AddAsync(user);

        }

        public async Task<string> GetDecryptedDataAsync(int userId)
        {
            // Retrieve the user from the database
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Decrypt the data
            return encryption.Decrypt(user.encryptedData);
        }
    }
}
