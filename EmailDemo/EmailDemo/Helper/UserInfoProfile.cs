using AutoMapper;
using EmailDemo.Model;
using EmailDemo.Model.Entity;

namespace EmailDemo.Helper
{
    public class UserInfoProfile:Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, UserInfoDTO>();
            CreateMap<UserInfoDTO, UserInfo>();
        }
    }
}
