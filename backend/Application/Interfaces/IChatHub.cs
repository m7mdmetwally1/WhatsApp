using Microsoft.AspNetCore.SignalR;
namespace Application.Interfaces;

public interface IChatHub 
{   
    public  Task SendMessage(string chatId,string userId, string message);
}