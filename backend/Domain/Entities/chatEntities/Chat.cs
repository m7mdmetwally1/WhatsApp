using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.chatEntities;

public class Chat 
{
 
      public string Id { get; set; }
    public string? Name { get; set; }

      public bool IsGroupChat { get; set; }

     public string? ImageUrl {get;set;}

     public ICollection<Messages> Messages { get; } = new List<Messages>();

       public List<ChatUser> ChatUser { get;set; } = [];
      
}

