
namespace Domain.Entities.chatEntities;


public class User
{
public string Id { get; set; }

public string FirstName { get; set; }

public string LastName { get; set; }

public string? NickName { get; set; }
    
public string? ImageUrl { get; set; }


 public ICollection<Messages> Messages { get; } = new List<Messages>();

public List<ChatUser> ChatUser { get; } = [];

}

