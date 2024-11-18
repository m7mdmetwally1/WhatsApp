namespace Application.ChatsDto;

public class CreateIndividualChatDto
{
    public required string SenderUserId { get; set; }
    public required string  SecondUserId { get; set; }
    public string? CustomName {get;set;}
}
