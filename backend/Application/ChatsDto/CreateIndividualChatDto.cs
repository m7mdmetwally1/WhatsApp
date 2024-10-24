namespace Application.ChatsDto;

public class CreateIndividualChatDto
{
    public required string SenderUserId { get; set; }
    public required string  SecondUserId { get; set; }
    public ChatType? ChatType {get;set;} = ChatsDto.ChatType.Individual;
    public string? CustomName {get;set;}
}

public enum ChatType
{
    Individual,
    Group
}