namespace Domain.Entities.chatEntities;

public class SeenBy
{
    public string Id { get; set; }

    public string MessagesId { get; set; } 
    public Messages Messages { get; set; }

    public DateTime SeenTime { get; set; }

    public string SeenWith {get;set;}



}