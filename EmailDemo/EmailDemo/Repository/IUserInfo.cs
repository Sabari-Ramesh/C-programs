using EmailDemo.Model.Entity;

namespace EmailDemo.Repository
{
    public interface IUserInfo
    {
        public Task<bool> AddUser(UserInfo userInfo);
        Task<bool> sendmailToAllUser();
    }
}
