using Domain.Entities.chatEntities;

namespace Application.ChatsDto;

public class MessageDto
{

    public string Id { get; set; }
    public string Content {get;set;}
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead {get;set;} = false;
    public string MessageSender { get; set; }
    public List<SeenBy> SeenBy {get;set;}

}

