namespace Domain.Entities.chatEntities;

public class GroupChatUser
{
    public required string GroupChatId { get; set; }
    public required string UserId { get; set; }
    public GroupChat GroupChat { get; set; } = null!;
    public User User { get; set; } = null!;
}







