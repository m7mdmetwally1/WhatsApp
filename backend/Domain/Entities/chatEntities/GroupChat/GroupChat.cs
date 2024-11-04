namespace Domain.Entities.chatEntities;

public class GroupChat
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageId { get; set; }
    public required string GroupCreatorId {get;set;}
    public List<User> Users { get; } = [];
    public required List<GroupChatUser> Members { get; set; } = new List<GroupChatUser>();
    public List<GroupMessage> Messages { get; set; } =  new List<GroupMessage>();
}

