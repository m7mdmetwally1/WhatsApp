using Domain.Entities.chatEntities;

namespace Application.ChatsDto;

public class InsertMessageDto
{


    public string ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content {get;set;}


}

