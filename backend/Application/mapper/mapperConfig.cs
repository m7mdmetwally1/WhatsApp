using AutoMapper;
using Domain.Entities;
using Application.UserDto;

namespace Application.mapper;

public class MapperConfig: Profile
{
    public MapperConfig()
    {

          CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        
    }
}
