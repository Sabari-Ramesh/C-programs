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
        public UserService(IUserRepository userRepository, AESEncryption encryption,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.encryption = encryption;
            this.mapper = mapper;
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
    }
}
