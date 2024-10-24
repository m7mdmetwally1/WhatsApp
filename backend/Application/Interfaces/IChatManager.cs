using Application.ChatsDto;
using Domain.Entities.chatEntities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IChatManager
{
    Task<bool> CreateChat(CreateIndividualChatDto chatDto);
    Task<bool> CreateGroupChat(CreateGroupChatDto chatDto);
    Task<GetChatResponse> GetIndividualChat(string senderUserIdstring ,string secondUserId);
    Task<bool> UpdateChatCustomName(string senderUserId,string customName,string chatId);
    Task<bool> AddImageUrlToDatabase(string imageUrl,string GroupChatId);
    Task<bool> AddUserImageUrlToDatabase(string imageUrl,string userId);
    Task<bool> DeleteGroupImageKitImage(string imageId,string groupChatId);
    Task<IEnumerable<GetIndividualChat>> GetUserIndividualChats(string userId);    
    Task<IEnumerable<GetGroupChat>> GetUserGroupChats(string userId);   
    Task<bool> DeleteFriend(CreateIndividualChatDto chatDto); 
}   
