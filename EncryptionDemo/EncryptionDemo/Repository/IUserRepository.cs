using EncryptionDemo.Entity;

namespace EncryptionDemo.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
    }
}
