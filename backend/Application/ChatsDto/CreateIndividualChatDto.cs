namespace Application.ChatsDto;

public class CreateIndividualChatDto
{
    public required string SenderUserId { get; set; }
    public  string?  FriendNumber { get; set; }
    public  string?  SecondUserId { get; set; }
    public string? CustomName {get;set;}
}
