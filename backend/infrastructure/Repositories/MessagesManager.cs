using Application.Interfaces;
using Application.ChatsDto;
using Application.Common;
using Domain.Entities.chatEntities;
using AutoMapper;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.SignalR;

namespace infrastructure.Repositories;

public class MessagesManager : IMessagesManager
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public MessagesManager(ApplicationDbContext context,IHubContext<ChatHub> hubContext,IMapper mapper, ILogger<MessagesManager> _logger)
    {
        this._context = context;
        this._hubContext = hubContext;
        this._mapper = mapper;
        this._logger = _logger;
    }
     
    public async Task<Result<bool>> InsertIndividualMessage(InsertIndividualMessageDto message)
    {                   
        var result = new Result<bool>();

        var user =await _context.User.Where(u=>u.Id == message.UserId ).FirstOrDefaultAsync();

        if(user == null)
        {            
            result.Success=false;
            result.Message="there is no user with this Id";            
            return result;
        }

        var chat = await _context.IndividualChat.Include(ic=>ic.IndividualChatUser).Where(c=>c.Id== message.ChatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no chat with this Id";            
            return result;
        }
                  
        var isMemeberOfTheChat = await _context.IndividualChatUser.Where(ic=>ic.UserId == message.UserId && ic.IndividualChatId == chat.Id).FirstOrDefaultAsync();

        if(isMemeberOfTheChat == null)
        {
            result.Success=false;
            result.Message="there is no user with this Id in this chat";            
            return result;
        }

        var messageToBeInserted = _mapper.Map<IndividualMessage>(message);
        messageToBeInserted.Id = Guid.NewGuid().ToString();
        messageToBeInserted.User= user;
        messageToBeInserted.Chat = chat;
        
        try
        {
            _context.IndividualMessages.Add(messageToBeInserted);            

            await _context.SaveChangesAsync();  

            await _hubContext.Clients.Group(message.ChatId).SendAsync("ReceiveMessage", message.ChatId, message.UserId, message.Content);   
                   
        }catch(Exception ex)
        {
            result.Success=false;           
            _logger.LogInformation($"{ex}"); 
            result.Message=$"Internal server";
            
            return result;
        }

            result.Message="message added to the chat";                        
            return result;                    
    }
  
     public async Task<Result<bool>> InsertGroupMessage(InsertGroupMessageDto message)
    {        
        var result = new Result<bool>();
        var user =await _context.User.Where(u=>u.Id == message.UserId ).FirstOrDefaultAsync();

        if(user == null)
        {
            result.Success=false;
            result.Message="there is no user with this Id";            
            return result;
        }

        var chat = await _context.Chat.Include(c=>c.Members).Where(c=>c.Id== message.ChatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no user with this Id";            
            return result;
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==message.UserId);

        if(!isMemeberOfTheChat)
        {
            result.Success=false;
            result.Message="there is no user with this Id in this chat";            
            return result;
        }

        var messageToBeInserted = _mapper.Map<GroupMessage>(message);
        messageToBeInserted.Id = Guid.NewGuid().ToString();
        messageToBeInserted.User= user;
        messageToBeInserted.GroupChat=chat;

        try
        {
            messageToBeInserted.SeenBy.Add(new SeenBy 
                { 
                    SeenTime = DateTime.UtcNow, 
                    SeenWith = user.Id, 
                    MessagesId = messageToBeInserted.Id, 
                    Id = user.Id 
                });

            _context.GroupMessages.Add(messageToBeInserted); 
            _logger.LogInformation($"{messageToBeInserted.SeenBy.Count()}");             

            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(message.ChatId).SendAsync("ReceiveMessage", message.ChatId, message.UserId, message.Content);

        }catch(Exception ex)
        {
            result.Success=false;
            _logger.LogInformation($"{ex}");
            result.Message=$"Internal server  {ex}";      
            return result;
        }

        result.Message="message added to the chat";               
        return result;
                                
    }

    public async Task<Result<bool>> OpenIndividualChat(string userId,string chatId)
    {
        var result = new Result<bool>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {
            result.Success=false;
            result.Message="there is no user with this id";            
            return result;
        }

        var chat = await _context.IndividualChat.Include(ic=>ic.IndividualChatUser).Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no chat with this id";            
            return result;
        }

         var isMemeberOfTheChat =  chat.IndividualChatUser.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            result.Success=false;
            result.Message="there is no user with this id in this chat";            
            return result;
        }

        try
        {
            var  addingResult =  await _context.IndividualMessages
            .Where(m => m.Chat.Id == chatId && m.User.Id != userId && !m.IsRead)
            .ExecuteUpdateAsync (m => m
            .SetProperty(msg => msg.IsRead, true)
            .SetProperty(msg => msg.SeenTime, DateTime.UtcNow));

            if(addingResult == 0) {
            result.Success=true;
            result.Message="there is no message to edit";            
            return result;
            }
         }
         catch(Exception ex)
         {
            result.Success=false;
            _logger.LogInformation($"{ex}");
            result.Message=$"Internal server  {ex}";      
            return result;
         }      
                               
        result.Message="messages now readed by the user";               
        return result;
    }

    public async Task<Result<bool>> OpenGroupChat(string userId,string chatId)
    {
       
        var result = new Result<bool>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {
            result.Success=false;
            result.Message="there is no user with this id";            
            return result;
        }


        var chat = await _context.Chat.Include(c=>c.Members).Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no chat with this id";            
            return result;
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            result.Success=false;
            result.Message="there is no user with this id in this chat";            
            return result;
        }

        try
        {
            var messages = await _context.GroupMessages.Include(gm=>gm.SeenBy).Where(m =>m.GroupChat.Id == chatId && !m.IsRead).ToListAsync();

            var groubMembersCount = chat.Members.Count(); 
            _logger.LogInformation($"{groubMembersCount}");          

            if(messages == null)
            {
                result.Success=false;                
                result.Message=$"all messages already seen";      
                return result;
            }

            foreach (var msg in messages)
            {                                
                var userSeenMe = msg.SeenBy.Where(s=>s.MessagesId==msg.Id && s.Id==userId).FirstOrDefault();                               
                 if(userSeenMe != null) continue;
                                            
                msg.SeenBy.Add(new SeenBy 
                { 
                    SeenTime = DateTime.UtcNow, 
                    SeenWith = user.Id, 
                    MessagesId = msg.Id, 
                    Id = user.Id 
                }); 

                if(groubMembersCount == msg.SeenBy.Count())
                {                    
                    msg.IsRead = true;
                }                         

            }                               

            await _context.SaveChangesAsync();

        }
        catch(Exception ex)
        {
            result.Success=false;
            _logger.LogInformation($"{ex}");
            result.Message=$"Internal server  {ex}";      
            return result;
        }
        result.Message="messages now read";
        return result;
    }

    public async Task<Result<GetIndividualMessagesDto>> IndividualChatMessages(string userId,string chatId)
    {
        var result = new Result<GetIndividualMessagesDto>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {  
            result.Success=false;
            result.Message=" there is no user with this Id ";
            return result;  
        }

        var chat = await _context.IndividualChat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no chat with this Id";
            return result; 
        }
          
        var isMemeberOfTheChat =  chat.IndividualChatUser.Where(ic=> ic.UserId == userId);

        if(isMemeberOfTheChat == null)
        {
            result.Success=false;
            result.Message="there is no user with this Id in the chat";
            return result;            
        }

        List<GetIndividualMessagesDto> messages;    

        try
        {
            messages = _context.IndividualMessages
                .Where(m => m.Chat.Id == chatId)
                .Select(m => new GetIndividualMessagesDto
                {
                    Content = m.Content,
                    IsRead = m.IsRead,
                    SentAt = m.SentAt,
                    SeenTime = m.SeenTime,
                    SenderId = m.User.Id
                })
                .ToList();
               
            result.Data=messages;    
          
        }
        catch (Exception ex)
        {           
            result.Message=$"Internal error occured ";
            _logger.LogInformation($"{ex}");
            result.Success=false;            
        }

        result.Message="getting messsages done";
        return result;
    }

    public async Task<Result<GetGroupMessagesDto>> GroupChatMessages(string userId,string chatId)
    {
        var result = new Result<GetGroupMessagesDto>();

        var user =await _context.User.Include(u=>u.Friends).Where(u=>u.Id == userId ).FirstOrDefaultAsync();       

        if(user == null)
        {  
            result.Success=false;
            result.Message="there is no user with this Id";            
            return result;  
        }      

        var chat = await _context.Chat.Include(c=>c.Members).Where(c=>c.Id== chatId).FirstOrDefaultAsync();
       
        if(chat == null)
        {
            result.Success=false;
            result.Message="there is no chat with this Id";            
            return result; 
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            result.Success=false;            
            result.Message="there is no chat with this Id in this chat";
            return result;
        }
        
        var friends = user.Friends.Select(f=>f.Id).ToList();

        List<GetGroupMessagesDto> messages ;
       
       try
       {
            messages = _context.GroupMessages.Include(Gm=>Gm.User).Where(m=> m.GroupChat.Id==chatId)                                                    
                                                    .Select(m=>new GetGroupMessagesDto{
                                                        Content=m.Content,
                                                        IsRead=m.IsRead,
                                                        SentAt=m.SentAt,                                                        
                                                        SenderId = m.User.Id,
                                                        SeenBy= m.SeenBy.Select( m=> new Seen{SeenTime= m.SeenTime ,SeenWith=m.SeenWith ?? ""}).ToList()
                                                    })
                                                    .ToList();            
        }catch(Exception ex)
        {
            result.Success=false;
            _logger.LogInformation($"{ex}");
            result.Message=$"Internal server error";            
            return result;
        }

        result.Data=messages;
        result.Message="getting messages done";                 
        return  result;
                                                                                                                                                                                                                                                                       
    }
    
    public async Task<Result<bool>> AddGroupMember(string userId,string chatId)
    {
        var result = new Result<bool>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {            
            result.Success=false;            
            result.Message="there is no user with this Id";
            return result;
        }       

        var chat = await _context.Chat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;            
            result.Message="there is no chat with this Id ";
            return result;
        }

        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(isMemeberOfTheChat)
        {
            result.Success=false;            
            result.Message="there is no chat with this Id in this chat";
            return result;
        }

        try
        {
            chat.Members.Add(new GroupChatUser{GroupChatId = chat.Id , UserId= userId});

            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            result.Success=false;            
            result.Message=$"Internal server error ";
            _logger.LogInformation($"{ex}");
            return result;
        }    

        result.Message="memeber added";                           
        return result;
    }

    public async Task<Result<bool>> ChangeIndividualChatMessageContent(string userId,string chatId,string messageId,string newContent)
    {
        var result = new Result<bool>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {            
            result.Success=false;            
            result.Message="there is no user with this id";
            return result;
        }       

        var chat = await _context.IndividualChat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;            
            result.Message="there is no chat with this id";
            return result;
        }

         var isMemeberOfTheChat =  chat.IndividualChatUser.Where(ic=> ic.UserId == userId);

        if(isMemeberOfTheChat ==null)
        {
            result.Success=false;            
            result.Message="there is no user with this id in this chat";
            return result;
        }

         var message =await _context.IndividualMessages.Where(im=> im.Id == messageId && im.User.Id==userId).FirstOrDefaultAsync(); 

         if(message == null)
         {
            result.Success=false;            
            result.Message="cant find this message";
            return result;
         }
         
         try
         {
            var allowedTime = message.SentAt.AddMinutes(5);

            if(DateTime.UtcNow <= allowedTime)
            {
                message.Content = newContent;                           
            }
            await _context.SaveChangesAsync();
         }
         catch(Exception ex)
         {
            result.Success=false;            
            result.Message=$"Internal server error ";
            _logger.LogInformation($"{ex}");
            return result;
         }
       
        result.Message="content changed";              
        return result;
    }

} 
    

