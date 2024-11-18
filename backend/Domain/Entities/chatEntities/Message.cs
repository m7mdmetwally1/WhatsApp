namespace Domain.Entities.chatEntities;

public class Message 
{
  public required string Id { get; set; }
  public required string Content {get;set;}
  public DateTime SentAt { get; set; } = DateTime.UtcNow;
  public bool IsRead { get; set; } = false;
  public DateTime SeenTime { get; set; } 
  public User User { get; set; } = null!;
}






