using AutoMapper;
using EncryptionDemo.DTO;
using EncryptionDemo.Entity;

namespace EncryptionDemo.Mapper
{
    public class UserProfiler:Profile
    {
        public UserProfiler() {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
