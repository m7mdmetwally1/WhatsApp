using Domain.Entities.chatEntities;

namespace Application.ChatsDto;

public class GetGroupChat
{
    public required List<string> Members { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl {get;set;}
    public string? LastMessage { get; set; }
    public bool CheckSeen { get; set; }
}