using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.chatEntities;

public class ChatUser
{
    public string ChatId { get; set; }
    public string UserId { get; set; }
    public Chat Chat { get; set; } = null!;
    public User User { get; set; } = null!;

    public string? CustomName { get; set; }
   
}




