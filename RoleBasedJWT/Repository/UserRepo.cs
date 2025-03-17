using RoleBasedJWT.Data;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Model.Entity;

namespace RoleBasedJWT.Repository
{
    public class UserRepo
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext context) { 
        this.context = context;
        }

        //Add User in DataBase
        public async Task<User> AddUser(User user)
        {
            if (user == null) {
                return null;
            }
          await context.users.AddAsync(user);
          context.SaveChanges();
          return user;
        }
    }
}
