using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Application.ChatsDto;

public class ChatDto
{

    public string Name { get; set; }
    
    public string? ImageUrl {get;set;}
    
    public required List<string> UserIds { get; set; }

    public string Sender { get; set; }

    public string otherSender {get;set;}

     public bool IsGroupChat { get; set; }

     public string? CustomName { get; set; }
    
    
}


