using BankApplicationJWT.Controllers;
using BankApplicationJWT.Data;
using BankApplicationJWT.Eception;
using BankApplicationJWT.Entity;

namespace BankApplicationJWT.Repo
{
    public class UserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context) { 
        _context = context;
        }
        public  String addUser(User user)
        {
            validateEmail(user.UserEmail);
            _context.Users.Add(user);
            _context.SaveChanges();
            return "User Sucessfully Added";
        }

        public  User FindUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.UserEmail == email);
        }


        //validate Email
        private void validateEmail(string userEmail)
        {
            if (_context.Users.Any(u => u.UserEmail == userEmail)) {
                throw new UserException("Email Already exit in data base");
            }
               
        }
    }
    }

