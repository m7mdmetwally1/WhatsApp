using AutoMapper;
using Application.ChatsDto;
using Domain.Entities;
using Application.UserDto;
using Domain.Entities.chatEntities;

namespace Application.mapper;

public class MapperConfig: Profile
{
    public MapperConfig()
    {

          CreateMap<ApiUserDto, ApiUser>().ReverseMap();
          CreateMap<ChatDto, Chat>().ReverseMap();
          CreateMap<MessageDto,Messages>().ReverseMap();
        
    }
}
