using AutoMapper;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Model.Entity;
using RoleBasedJWT.Repository;

namespace RoleBasedJWT.Service
{
    public class UserService
    {
        private readonly UserRepo userrepo;

        private readonly IMapper mapper;

        //Constructor
        public UserService(UserRepo userRepo , IMapper mapper) { 
         this.userrepo = userRepo;
        this.mapper = mapper;
        }

        //Add User in Repository
        public async Task<UserDto> AddUser(UserDto user)
        {
            var saveUser = mapper.Map<User>(user);
            User resUser = await userrepo.AddUser(saveUser);
            var resUserDTO = mapper.Map<UserDto>(resUser);
            return resUserDTO;
        }
    }
}
