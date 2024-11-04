using Application.ChatsDto;
using Application.Common;

namespace Application.Interfaces;

public interface IMessagesManager
{
    Task<CustomReturn> InsertIndividualMessage(InsertIndividualMessageDto message);
    Task<CustomReturn> InsertGroupMessage(InsertGroupMessageDto message);
    Task<CustomReturn> OpenIndividualChat(string userId,string chatId);
    Task<CustomReturn> OpenGroupChat(string userId,string chatId);
    Task<Result<GetIndividualMessagesDto>> IndividualChatMessages(string chatId,string userId);
    Task<Result<GetGroupMessagesDto>> GroupChatMessages(string chatId,string userId);
    Task<bool> ChangeIndividualChatMessageContent(string chatId,string userId,string messageId,string newContent);
    Task<bool> AddGroupMember(string chatId,string userId);    
}
