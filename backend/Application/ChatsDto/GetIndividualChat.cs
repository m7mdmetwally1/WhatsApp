using Domain.Entities.chatEntities;

namespace Application.ChatsDto;
public class GetIndividualChat
{
  public string? UserImageKitUrl { get; set; }
  public string? CustomName { get; set; }
  public string? LastMessage { get; set; }
  public bool? CheckSeen {get;set;}
}