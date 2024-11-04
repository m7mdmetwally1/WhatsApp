namespace Application.ChatsDto;

public class CreateGroupChatDto
{
    public required List<string> GroupChatUsers { get; set; }
    public ChatType? ChatType {get;set;} = ChatsDto.ChatType.Group;
    public string? Name {get;set;}
    public required string GroupChatCreator { get; set; }
}


