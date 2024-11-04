namespace Domain.Entities.chatEntities;

public class SeenBy
{
    public required string Id { get; set; }
    public required string MessagesId { get; set; } 
    public  GroupMessage Messages { get; set; } = null!;
    public DateTime SeenTime { get; set; }
    public  string? SeenWith {get;set;}
}
