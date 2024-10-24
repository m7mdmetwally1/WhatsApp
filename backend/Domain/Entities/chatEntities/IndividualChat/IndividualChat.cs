using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.chatEntities;

public class IndividualChat
{
  public required string Id { get; set; }
  public List<User> Users { get; } = [];
  public ICollection<IndividualMessage> Messages { get; } = new List<IndividualMessage>();
  public required List<IndividualChatUser> IndividualChatUser { get; set; } = new List<IndividualChatUser>();
}

