namespace Application.ChatsDto;

public class CreateGroupChatDto
{
    public required List<string> GroupChatUsers { get; set; }
    public string? Name {get;set;}
    public required string GroupChatCreator { get; set; }
}


