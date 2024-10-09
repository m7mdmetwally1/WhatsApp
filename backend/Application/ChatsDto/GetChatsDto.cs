namespace Application.ChatsDto;

public class GetChatsDto
{
    
    public List<UserDto> Users { get; set; }
    public bool IsGroupChat { get; set; }
    public String GroupName { get; set; }
    public string GroupImageUrl { get; set; }
    
    public string LastMessage { get; set; }

}

