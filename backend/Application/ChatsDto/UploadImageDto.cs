namespace Application.ChatsDto;

public class UploadImageDto 
{
    public string? UserId { get; set; }
 
    public string? GroupChatId { get; set; }
    public required IFormFile ImageUrl { get; set; }

}
   