using Application.ChatsDto;
using Application.Common;

namespace Application.Interfaces;

public interface IMessagesManager
{
    Task<Result<bool>> InsertIndividualMessage(InsertIndividualMessageDto message);
    Task<Result<bool>> InsertGroupMessage(InsertGroupMessageDto message);
    Task<Result<bool>> OpenIndividualChat(string userId,string chatId);
    Task<Result<bool>> OpenGroupChat(string userId,string chatId);
    Task<Result<GetIndividualMessagesDto>> IndividualChatMessages(string chatId,string userId);
    Task<Result<GetGroupMessagesDto>> GroupChatMessages(string chatId,string userId);
    Task<Result<bool>> ChangeIndividualChatMessageContent(string chatId,string userId,string messageId,string newContent);
    Task<Result<bool>> AddGroupMember(string chatId,string userId);    
}
