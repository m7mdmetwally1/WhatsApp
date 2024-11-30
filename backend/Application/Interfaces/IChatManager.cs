using Application.ChatsDto;
using Application.Common;
using Domain.Entities.chatEntities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IChatManager
{
    Task<Result<string>> CreateChat(CreateIndividualChatDto chatDto);
    Task<Result<bool>> CreateGroupChat(CreateGroupChatDto chatDto);
    Task<Result<IndividualChat>> GetIndividualChat(string senderUserIdstring ,string secondUserId);
    Task<Result<bool>> UpdateChatCustomName(string senderUserId,string customName,string chatId);
    Task<Result<bool>> AddGroupImageUrlToDatabase(string imageUrl,string imageId,string GroupChatId);
    Task<Result<bool>> AddUserImageUrlToDatabase(string imageUrl,string imageId,string userId);
    Task<Result<bool>> DeleteGroupImageKitImage(string imageId,string groupChatId);
    Task<Result<GetChat>> GetUserIndividualChats(string userId);    
    Task<Result<GetChat>> GetUserGroupChats(string userId);   
    Task<Result<bool>> DeleteFriend(CreateIndividualChatDto chatDto); 
    Task<Result<bool>> AddGroupMember(string chatId,string userId,string creator);  
}   

