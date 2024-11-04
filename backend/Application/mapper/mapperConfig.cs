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
          CreateMap<CreateIndividualChatDto,IndividualChat>().ReverseMap();
          CreateMap<CreateGroupChatDto,GroupChat>().ReverseMap();
          CreateMap<InsertIndividualMessageDto,IndividualMessage>().ReverseMap();
          CreateMap<InsertGroupMessageDto,GroupMessage>().ReverseMap();
        //   CreateMap<ChatDto, Chat>().ReverseMap();
        //   CreateMap<MessageDto,Messages>().ReverseMap();
        
    }
}
