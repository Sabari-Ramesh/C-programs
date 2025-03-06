using AutoMapper;
using EncryptionDemo.DTO;
using EncryptionDemo.Entity;
using EncryptionDemo.Helper;
using EncryptionDemo.Repository;

namespace EncryptionDemo.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly AESEncryption encryption;
        public readonly IMapper mapper;
        public readonly RSAEncryption rsaEncryption;
        public UserService(IUserRepository userRepository, AESEncryption encryption,IMapper mapper,RSAEncryption rSAEncryption)
        {
            this.userRepository = userRepository;
            this.encryption = encryption;
            this.mapper = mapper;
            this.rsaEncryption = rSAEncryption;
        }

        public async Task addUser(string userName,string userInformation) { 
            //Encrypt the Data
            String encryptedDataInfo = encryption.Encrypt(userInformation);
            //Console.WriteLine(encryptedDataInfo);
            //Console.WriteLine(userName);

            //var user = new User
            //{
            //    name = userName,
            //    encryptedData = encryptedDataInfo
            //};

            UserDTO userDto = new()
            {
                name = userName,
                encryptedData = encryptedDataInfo
            };
            var user = mapper.Map<User>(userDto);
            Console.WriteLine(user.name);
            Console.WriteLine(user.encryptedData);
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
            var userDTO =mapper.Map<UserDTO>(user);

            // Decrypt the data
            return encryption.Decrypt(userDTO.encryptedData);
        }


        //Encryption Using the RSA
        public async Task addUserRSAEncryption(string userName, string userInformation)
        {
            String encryptedDataInfo = rsaEncryption.Encryption(userInformation);
            UserDTO userDto = new()
            {
                name = userName,
                encryptedData = encryptedDataInfo
            };
            var user = mapper.Map<User>(userDto);
            Console.WriteLine(user.name);
            Console.WriteLine(user.encryptedData);
            await userRepository.AddAsync(user);
        }

        //Decryption Using the RSA
        public async Task<string> DecryptedUsingRSA(int userId) {

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            var userDTO = mapper.Map<UserDTO>(user);

            // Decrypt the data
            return rsaEncryption.Decryption(userDTO.encryptedData);
        }
    }
}
