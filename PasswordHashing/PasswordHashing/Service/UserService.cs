
using BCrypt.Net;
using PasswordHashing.Data;
namespace PasswordHashing.Service
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context) { 
        _context = context; 
        }
        public bool addUser(string userName, string password)
        {
            if (_context.Users.Any(u => u.Username == userName))
            {
                throw new Exception("Username already exists.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = userName,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;  
        }

        public bool loginUser(string? userName, string? password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                return false; // User not found
            }

            // Verify the password
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
