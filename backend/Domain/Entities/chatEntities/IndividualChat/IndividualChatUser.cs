namespace Domain.Entities.chatEntities;

public class IndividualChatUser
{
public required string IndividualChatId { get; set; }
public required string UserId { get; set; }
public IndividualChat IndividualChat { get; set; } =null!;
public User User { get; set; } =null!;
public string? CustomName { get; set; } 
}


