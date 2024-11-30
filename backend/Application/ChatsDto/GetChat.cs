using Domain.Entities.chatEntities;

namespace Application.ChatsDto;

public class GetChat
{    
    public string? CustomName { get; set; }
    public string? ImageUrl {get;set;}
    public string? LastMessage { get; set; }
    public DateTime? SentTime { get; set; }
    public int? NumberOfUnSeenMessages { get; set; }
    public string? Number { get; set; }
    public required string ChatId { get; set; }
    public required bool ChatType { get; set; }
}


