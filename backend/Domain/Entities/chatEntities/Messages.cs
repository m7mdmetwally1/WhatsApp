namespace Domain.Entities.chatEntities;

public class Messages 
{
     public string Id { get; set; }
    public string Content {get;set;}
   public DateTime SentAt { get; set; } 
    public bool IsRead { get; set; } = false;

      public DateTime SeenTime { get; set; }
     public User User { get; set; } = null!;
     public Chat Chat { get; set; } = null!;

      public List<SeenBy> SeenBy { get; set; } = new List<SeenBy>();

}





