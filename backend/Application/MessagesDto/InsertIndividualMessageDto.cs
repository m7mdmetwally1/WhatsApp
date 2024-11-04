namespace Application.ChatsDto;

public class InsertIndividualMessageDto
{
    public required string Content { get; set; }
    public required string UserId { get; set; }
    public required string ChatId { get; set; }
}