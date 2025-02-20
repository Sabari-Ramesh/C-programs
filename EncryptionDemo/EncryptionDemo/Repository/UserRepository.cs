using EncryptionDemo.Data;
using EncryptionDemo.Entity;
using Microsoft.EntityFrameworkCore;

namespace EncryptionDemo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        //Add User

        public async Task AddAsync(User user)
        {
          await context.Users.AddAsync(user);
          context.SaveChanges();

        }

        //Find By Id User

        public async Task<User> GetByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }
    }
}
