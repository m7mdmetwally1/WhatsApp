namespace Application.ChatsDto;

public class GetIndividualMessagesDto
{
    public required string Content { get; set; }
    public required DateTime SeenTime { get; set; }
    public required bool IsRead { get; set; }
    public  DateTime? SentAt { get; set; }
    public required string SenderId { get; set; }
      
}


// 