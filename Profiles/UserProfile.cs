using tripAdvisorAPI.Models;
using tripAdvisorAPI.DTO;
using tripAdvisorAPI.DTO.Trip;
using AutoMapper;

namespace tripAdvisorAPI.Profiles;

public class UserProfile : Profile
{
    

    public UserProfile()
    {
        CreateMap<UserDTO, User>();

        CreateMap<CreateUserDTO, User>();

        CreateMap<User, UserDTO>();

    }
    
    
}