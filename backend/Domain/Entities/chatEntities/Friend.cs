namespace Domain.Entities.chatEntities;

public class Friend
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public User User {get;set;} = null!;
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public string? CustomName { get; set; }
    public string? ImageUrl { get; set; }
}


