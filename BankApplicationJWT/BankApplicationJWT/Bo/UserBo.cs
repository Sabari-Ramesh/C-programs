using BankApplicationJWT.Controllers;
using BankApplicationJWT.Data;
using BankApplicationJWT.Eception;
using BankApplicationJWT.Entity;
using BankApplicationJWT.Repo;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BankApplicationJWT.Bo
{
    public class UserBo
    {
        private readonly UserRepo _userRepo;

        public UserBo(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public string addUser(User user)
        {
           validateName(user.UserName);
            validateUserEmail(user.UserEmail);
            string resmsg = _userRepo.addUser(user);
            return resmsg;
        }

        public User Login(LoginController.LoginRequest request)
        {
            validateUserEmail(request.UserEmail);
            validateUserPassword(request.UserPassword);
            User user = _userRepo.FindUser(request.UserEmail);
            if (!request.UserPassword.Equals(user.Password)) {
                throw new UserException("Invalid User Password!!!");
            }
            return user;
        }


        //Validation

        private void validateName(string userName)
        {
            if (userName == null || userName.Length == 0) {
                throw new UserException("Enter Valid Name Only");
            }
            if (!Regex.IsMatch(userName, @"^[A-Za-z\s]+$")) {
                throw new UserException("Name Must be contain the Alphabet and the space only.");
            }
        }

        private void validateUserPassword(string userPassword)
        {
            if (userPassword == null || userPassword.Length <= 1) {
                throw new UserException("Please Enter Valid Passord");
            }
        }

        private void validateUserEmail(string userEmail)
        {
            if (!userEmail.Contains("@gmail.com"))
            {
                throw new UserException("In valid Email .Email must contain the @ gmail.com");
            }

        } 
    }
}
