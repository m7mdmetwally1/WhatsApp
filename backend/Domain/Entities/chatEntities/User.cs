namespace Domain.Entities.chatEntities;

public class User
{
    public required string Id { get; set; }
    public required string  FirstName { get; set; } 
    public required string LastName { get; set; }
    public string? ImageUrl {get;set;}
    public List<Friend> Friends {get;set;} = new List<Friend>();
    public List<GroupChat> GroupChats { get; } = [];
    public List<IndividualChat> IndividualChats { get; } = [];
    public List<IndividualChatUser> GroupChatUsers { get; set; } = new List<IndividualChatUser>();
    public List<GroupChatUser> IndividualChatUsers { get; set; } = new List<GroupChatUser>();
}





