using AutoMapper;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Model.Entity;

namespace RoleBasedJWT.Helper
{
    public class MapperHelper:Profile
    {
        public MapperHelper() {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
        }
    }
}
