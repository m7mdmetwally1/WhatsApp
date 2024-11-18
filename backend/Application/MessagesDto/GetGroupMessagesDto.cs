using Domain.Entities.chatEntities;

namespace Application.ChatsDto;

public class GetGroupMessagesDto
{
    public required string Content { get; set; }
    public required bool IsRead { get; set; }
    public  DateTime? SentAt { get; set; }
    public required string SenderName { get; set; }
    public List<Seen>? SeenBy { get; set; } 
}

public class Seen
{
    public DateTime? SeenTime { get; set; }
    public required  string SeenWith {get;set;}
}

