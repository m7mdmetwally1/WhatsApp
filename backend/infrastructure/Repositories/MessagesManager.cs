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

namespace infrastructure.Repositories;

public class MessagesManager : IMessagesManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public MessagesManager(ApplicationDbContext context, IMapper mapper, ILogger<MessagesManager> _logger)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = _logger;
    }
    public async Task<CustomReturn> InsertIndividualMessage(InsertIndividualMessageDto message)
    {                   
        var user =await _context.User.Where(u=>u.Id == message.UserId ).FirstOrDefaultAsync();

        if(user == null)
        {
            throw new ValidationException("there is no user with this id");
        }


        var chat = await _context.IndividualChat.Include(ic=>ic.IndividualChatUser).Where(c=>c.Id== message.ChatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            throw new ValidationException("there is no chat with this id");
        }
                  
        var isMemeberOfTheChat = await _context.IndividualChatUser.Where(ic=>ic.UserId == message.UserId && ic.IndividualChatId == chat.Id).FirstOrDefaultAsync();

        if(isMemeberOfTheChat == null)
        {
            throw new ValidationException("the given user is not in the given chat");
        }

        var messageToBeInserted = _mapper.Map<IndividualMessage>(message);
        messageToBeInserted.Id = Guid.NewGuid().ToString();
        messageToBeInserted.User= user;
        messageToBeInserted.Chat = chat;
        
        try
        {
            _context.IndividualMessages.Add(messageToBeInserted);

            var result = await _context.SaveChangesAsync();

        }catch(Exception ex)
        {
            throw new InternalServerErrorException($"{ex.Message}");
        }
               
        return new CustomReturn{StatusCode=500,Message="failed to add message"};
          
    }

     public async Task<CustomReturn> InsertGroupMessage(InsertGroupMessageDto message)
    {        
        var user =await _context.User.Where(u=>u.Id == message.UserId ).FirstOrDefaultAsync();

        if(user == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no user with this id"};
        }

        var chat = await _context.Chat.Include(c=>c.Members).Where(c=>c.Id== message.ChatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no chat with this id"};
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==message.UserId);

        if(!isMemeberOfTheChat)
        {
            return new CustomReturn{StatusCode=400,Message="user is not a member of this chat"};
        }

        var messageToBeInserted = _mapper.Map<GroupMessage>(message);
        messageToBeInserted.Id = Guid.NewGuid().ToString();
        messageToBeInserted.User= user;
        messageToBeInserted.GroupChat=chat;

         _context.GroupMessages.Add(messageToBeInserted);

        var result = await _context.SaveChangesAsync();

        if(result > 0)
        {
            return new CustomReturn{StatusCode=200,Message="success"};
        }

        return new CustomReturn{StatusCode=500,Message="failed to add message"};
          
    }

    public async Task<CustomReturn> OpenIndividualChat(string userId,string chatId)
    {
        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no user with this id"};
        }

         var chat = await _context.IndividualChat.Include(ic=>ic.IndividualChatUser).Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no chat with this id"};
        }

         var isMemeberOfTheChat =  chat.IndividualChatUser.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            return new CustomReturn{StatusCode=400,Message="user is not a member of this chat"};
        }
               
            var  result =  await _context.IndividualMessages
        .Where(m => m.Chat.Id == chatId && m.User.Id != userId && !m.IsRead)
        .ExecuteUpdateAsync (m => m
        .SetProperty(msg => msg.IsRead, true)
        .SetProperty(msg => msg.SeenTime, DateTime.UtcNow));
                
        if(result > 0)
        {
            return new CustomReturn{StatusCode=200,Message="success"};
        }

        return new CustomReturn{StatusCode=500,Message="failed to open chat"};
    }

    public async Task<CustomReturn> OpenGroupChat(string userId,string chatId)
    {
        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no user with this id"};
        }


        var chat = await _context.Chat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            return new CustomReturn {StatusCode=400,Message ="there is no chat with this id"};
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            return new CustomReturn{StatusCode=400,Message="user is not a member of this chat"};
        }

        var messages = await _context.GroupMessages.Where(m =>m.GroupChat.Id == chatId && !m.IsRead).ToListAsync();
        var groubMembersCount = chat.Members.Count();

        foreach (var msg in messages)
        {
            msg.SeenBy.Add(new SeenBy 
            { 
                SeenTime = DateTime.UtcNow, 
                SeenWith = user.FirstName, 
                MessagesId = msg.Id, 
                Id = user.Id 
            });

            if(groubMembersCount == msg.SeenBy.Count())
            {
                msg.IsRead = true;
            }
        }                               

        var result = await _context.SaveChangesAsync();

        if(result > 0)
        {
            return new CustomReturn{StatusCode=200,Message="success"};
        }

        return new CustomReturn{StatusCode=500,Message="failed to add message"};
    }

    public async Task<Result<GetIndividualMessagesDto>> IndividualChatMessages(string userId,string chatId)
    {
        var result = new Result<GetIndividualMessagesDto>();

        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {  
            result.Success=false;
            result.ErrorMessage="there is no user with this Id";
            return result;  
        }

        var chat = await _context.IndividualChat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.ErrorMessage="there is no chat with this Id";
            return result; 
        }
          
        var isMemeberOfTheChat =  chat.IndividualChatUser.Where(ic=> ic.UserId == userId);

        if(isMemeberOfTheChat == null)
        {
            result.Success=false;
            result.ErrorMessage="there is no user with this Id in the chat";
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
                    SenderName = m.User.FirstName
                })
                .ToList();

            result.StatusCode=200;    
            result.Data=messages;    
          
        }
        catch (Exception ex)
        {           
            result.ErrorMessage="Internal Error occured";
            result.Success=false;
            result.StatusCode = 500;
        }
    
        return result;
    }

    public async Task<Result<GetGroupMessagesDto>> GroupChatMessages(string userId,string chatId)
    {
        var result = new Result<GetGroupMessagesDto>();

        var user =await _context.User.Include(u=>u.Friends).Where(u=>u.Id == userId ).FirstOrDefaultAsync();       

        if(user == null)
        {  
            result.Success=false;
            result.ErrorMessage="there is no user with this Id";
            result.StatusCode=400;
            return result;  
        }      

        var chat = await _context.Chat.Include(c=>c.Members).Where(c=>c.Id== chatId).FirstOrDefaultAsync();
       
        if(chat == null)
        {
            result.Success=false;
            result.ErrorMessage="there is no chat with this Id";
            result.StatusCode=400;
            return result; 
        }
          
        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(!isMemeberOfTheChat)
        {
            result.Success=false;
            result.StatusCode=400;
            result.ErrorMessage="there is no chat with this Id in this chat";
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
                                                        SenderName = m.User.FirstName,
                                                        SeenBy= m.SeenBy.Select( m=> new Seen{SeenTime= m.SeenTime ,SeenWith=m.SeenWith ?? ""}).ToList()
                                                    })
                                                    .ToList();
            result.Data=messages;
            result.StatusCode=200;

        }catch(Exception ex)
        {
            result.Success=false;
            result.ErrorMessage="there is no chat with this Id";
            result.StatusCode=500;
            return result;
        }
                    
          return  result;
                                                                                                                                                                                                                                                                       
    }

    public async Task<bool> AddGroupMember(string userId,string chatId)
    {
        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {            
            throw new ArgumentException("There is no user with this ID", nameof(userId));
        }       

        var chat = await _context.Chat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            throw new ArgumentException("There is no chat with this ID", nameof(chatId));
        }

        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(isMemeberOfTheChat)
        {
            throw new ArgumentException("user already member of the group", nameof(userId));
        }

        chat.Members.Add(new GroupChatUser{GroupChatId = chat.Id , UserId= userId});

        var result = await _context.SaveChangesAsync();

       if(result == 0) throw new ArgumentException("failed to add to group", nameof(userId));
       
       return result > 0;
    }

      public async Task<bool> ChangeIndividualChatMessageContent(string userId,string chatId,string messageId,string newContent)
    {
        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();

        if(user == null)
        {            
            throw new ArgumentException("There is no user with this ID", nameof(userId));
        }       

        var chat = await _context.IndividualChat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            throw new ArgumentException("There is no chat with this ID", nameof(chatId));
        }

         var isMemeberOfTheChat =  chat.IndividualChatUser.Where(ic=> ic.UserId == userId);

        if(isMemeberOfTheChat ==null)
        {
            throw new ArgumentException("user not in this chat", nameof(userId));
        }

         var message =await _context.IndividualMessages.Where(im=> im.Id == messageId && im.User.Id==userId).FirstOrDefaultAsync(); 

         if(message == null)
         {
            throw new ArgumentException("cant find this message");
         }

         var allowedTime = message.SentAt.AddMinutes(5);

         if(DateTime.UtcNow <= allowedTime)
         {
            message.Content = newContent;
            var result = await _context.SaveChangesAsync();
            if(result == 0) throw new ArgumentException("failed to add to group", nameof(userId));
            return result > 0;
         } 

       throw new ArgumentException("cant edit content");
    }

} 
    

