namespace Domain.Entities.chatEntities;

public class GroupMessage : Message
{
  public GroupChat GroupChat { get; set; } = null!;
  public List<SeenBy> SeenBy { get; set; } = new List<SeenBy>();
}





