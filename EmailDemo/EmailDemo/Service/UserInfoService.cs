using EmailDemo.Model.Entity;
using EmailDemo.Repository;

namespace EmailDemo.Service
{
    public class UserInfoService
    {
        private readonly IUserInfo userInfoRep;

       public UserInfoService(IUserInfo userInfo)
        {
            this.userInfoRep = userInfo;
        }

        public async Task<bool> AddUserInfo(UserInfo userInfo)
        {
            bool flag = false;
           flag = await  userInfoRep.AddUser(userInfo);
            return flag;
        }

        public async Task<bool> sendMailToAllUser()
        {
           return await userInfoRep.sendmailToAllUser();
        }

        public async Task<bool> validateEmail(string email, string token)
        {
            Console.WriteLine($"service - Email: {email}, Token: {token}");
            return await userInfoRep.validateEmail(email,token);
        }
    }
}
