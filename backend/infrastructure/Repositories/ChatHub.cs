using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Application.Interfaces;
using System.Collections.Concurrent;
using infrastructure.Data;
using Application.ChatsDto;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Entities.chatEntities;


namespace infrastructure.Repositories;

[Authorize]
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly ApplicationDbContext _context;

    // private static readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

    public ChatHub(ILogger<ChatHub> logger,ApplicationDbContext context) 
    {
        this._logger = logger;
        this._context = context;
    }

    public override async Task OnConnectedAsync()
    {           
        try
        {
            await JoinChatGroupsForUser();
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while joining chat groups: {ex.Message}");
        }
    }

    public async Task JoinChatGroupsForUser()
    {              
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        var chats = await _context.IndividualChat                                        
                    .Where(c => c.IndividualChatUser.Any(u => u.UserId == userId))
                    .ToListAsync();
                    
         chats.Add(new IndividualChat{Id=userId,IndividualChatUser=null});

        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("User not authenticated.");
            _logger.LogInformation($"inside hub failed");
            return;
        }
        
        foreach (var chat in chats)
        {        
            _logger.LogInformation($"{chat}");
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id);        
        }

        //  _userConnections.Add(userId, Context.ConnectionId);
    }

 

    //   public static string? GetConnectionIdForUser(string userId)
    // {
    //     return _userConnections.ContainsKey(userId) ? _userConnections[userId] : null;
    // }
   
}


