namespace Application.ChatsDto;

public class UserDto
{
    public required string FirstName { get; set; }
    public string? ImageUrl { get; set; } 
    public string? CustomName { get; set; } 
    public string? LastMessage { get; set; }
}

