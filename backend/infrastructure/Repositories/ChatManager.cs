using Application.ChatsDto;
using Application.Common;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.chatEntities;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Org.BouncyCastle.Asn1.Icao;
using Org.BouncyCastle.Crypto.Macs;

namespace infrastructure.Repositories;

public class ChatManager : IChatManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IImageKitService _imageKitService;
    private readonly IAuthMangaer _authMangaer;
    private readonly ILogger<ChatManager> _logger;
  

    public ChatManager(ApplicationDbContext context,IMapper mapper,IImageKitService imageKitService,IAuthMangaer authMangaer,ILogger<ChatManager> logger )
  {
      this._context = context;
      this._mapper = mapper;
      this._imageKitService = imageKitService;
      this._authMangaer = authMangaer;
      this._logger = logger;
  }
  public async Task<Result<string>> CreateChat(CreateIndividualChatDto chatDto)
  {
      var result = new Result<string>();

      var newChat = _mapper.Map<IndividualChat>(chatDto);
    
      newChat.Id = Guid.NewGuid().ToString();
      newChat.IndividualChatUser.Add(new IndividualChatUser{UserId = chatDto.SenderUserId,IndividualChatId = newChat.Id,CustomName = chatDto.CustomName ?? ""});
      newChat.IndividualChatUser.Add(new IndividualChatUser{UserId = chatDto.SecondUserId,IndividualChatId = newChat.Id});
      
      try
      {
        _context.IndividualChat.Add(newChat);  
        await _context.SaveChangesAsync();
      }
      catch(Exception ex)
      {
        result.Success=false;
        _logger.LogInformation($"{ex}");
        result.Message=$"Internal server error";        
        return result;
      } 
  
      result.SingleData=newChat.Id;  
      result.Message="chat created successfully";
      return result;
  }
    public async Task<Result<bool>> CreateGroupChat(CreateGroupChatDto chatDto)
  {
    var result = new Result<bool>();

    foreach(var userId in chatDto.GroupChatUsers){
      var user =await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync();
      if(user == null){
        result.Success=false;
        result.Message=$"there is no user for the given id {userId}";        
        return result;
      }
    }

    var GroupChat = _mapper.Map<GroupChat>(chatDto);
          
    GroupChat.Id = Guid.NewGuid().ToString();

    var creator = await _context.User.Where(u=>u.Id == chatDto.GroupChatCreator).FirstOrDefaultAsync();

    if(creator == null)
    {
      result.Success=false;
      result.Message="there is no user with this Id for the creator";      
      return result;
    }

    foreach(var userId in chatDto.GroupChatUsers){
      var existingMember = GroupChat.Members.FirstOrDefault(m => m.UserId == userId);
      if(existingMember == null){
          GroupChat.Members.Add(new GroupChatUser{UserId = userId,GroupChatId =GroupChat.Id});
      }      
    } 
    var exist =chatDto.GroupChatUsers.Contains(chatDto.GroupChatCreator);

    if(!exist) GroupChat.Members.Add(new GroupChatUser{UserId=creator.Id ,GroupChatId=GroupChat.Id});
    
    GroupChat.GroupCreatorId= creator.Id; 
    try
    {
      _context.Chat.Add(GroupChat);

     await _context.SaveChangesAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }
            
    result.Message="chat created successfully";
    return result;
  }

  public async Task<Result<IndividualChat>> GetIndividualChat(string senderUserId,string secondUserId)
  {
    var result = new Result<IndividualChat>();

    IndividualChat? Chat;

    try
    {
       Chat =await _context.IndividualChat
                        .Where(c => c.IndividualChatUser.Any(c=>c.UserId == senderUserId) && c.IndividualChatUser.Any(c=> c.UserId == secondUserId))
                        .FirstOrDefaultAsync();

      if(Chat == null){
          result.Success=false;
          result.Message="there is no chat with this id";
          
          return result;
      }                        
    }
    catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }      
      result.SingleData = Chat;               
      return result;                
  }

  public async Task<Result<bool>> UpdateChatCustomName(string senderUserId,string customName,string chatId )
  {
    var result = new Result<bool>();

    var chatUser = await _context.IndividualChatUser
                                .Where(cu => cu.UserId == senderUserId && cu.IndividualChatId == chatId)
                                .FirstOrDefaultAsync();
                                 
    if(chatUser == null){
      result.Success=false;
      result.Message="the user is not in the chat";
      
      return result;
    }

    try
    {
      chatUser.CustomName = customName;
       await _context.SaveChangesAsync();
    }
    catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }  

      result.Message="chat custom name updated successfully";
      return result;
  }
 
  public async Task<Result<bool>> AddGroupImageUrlToDatabase(string imageUrl,string imageId,string groupChatId)
  {
    var result = new Result<bool>();
    
    if(imageUrl == null){
      result.Success=false;
      result.Message="you shoould provide image";
      
      return result;
    }

    var GroupChat =await _context.Chat.Where(c=> c.Id == groupChatId).FirstOrDefaultAsync();

    if(GroupChat == null){
      result.Success=false;
      result.Message="there is no group with this id";
      
      return result;
    }

    GroupChat.ImageUrl = imageUrl;
    GroupChat.ImageId =imageId;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }
      
      result.Message="image url add to data base successfully";
      return result;         
  }

  public async Task<Result<bool>> DeleteGroupImageKitImage(string imageId,string groupChatId)
  {
    var result = new Result<bool>();

    var groupChat = await _context.Chat.Where(c=>c.Id == groupChatId).FirstOrDefaultAsync();

    if(groupChat == null){
      result.Success=false;
      result.Message="there is no groupChat with this id";
      
      return result;
    }

    groupChat.ImageUrl = "";    
    groupChat.ImageId = "";    

    try
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

     await _context.SaveChangesAsync();
     await _imageKitService.DeleteImage(imageId);

     await transaction.CommitAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }
     
    result.Message="image deleted successfully";
    return result;
  }
  
  public async Task<Result<GetChat>> GetUserIndividualChats(string userId)
  {
      var result = new Result<GetChat>();

      var user =await _context.User.Where(u=> u.Id == userId).FirstOrDefaultAsync();
      if(user == null) {
        result.Success=false;
        result.Message=$"there is no user with this id";      
        return result;
      } 
      
      List<GetChat> chats;
      try
      {
       chats =await _context.IndividualChat.Include(c=>c.Messages).Where(c=> c.IndividualChatUser.Any(u => u.UserId == userId))
      .Select(c => new GetChat {
      CustomName=c.IndividualChatUser.Where(ic=>ic.UserId == userId).Select(ic=>ic.CustomName).FirstOrDefault(),
      Number=c.Users.Where(c=>c.Id != userId).Select(u => u.Number).FirstOrDefault(),
      ImageUrl = c.IndividualChatUser.Where(ic=>ic.UserId != userId).Select(ic=>ic.User.ImageUrl ).FirstOrDefault(),
      LastMessage = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.Content).FirstOrDefault(),      
      SentTime = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.SentAt).FirstOrDefault(),
      NumberOfUnSeenMessages=c.Messages.Where(m=>!m.IsRead && m.User.Id != userId).Count(),
      ChatId=c.Id, 
      ChatType=true
      }
      )
      .ToListAsync();

      }catch(Exception ex)
      {
        result.Success=false;
        result.Message=$"Internal server  ${ex}";
        
        return result;
      }            
      
      result.Data=chats;
      result.Message="getting chats success";
      return result;       
  }
  
  public async Task<Result<GetChat>> GetUserGroupChats(string userId)
  {
    var result = new Result<GetChat>();

    var user =await _context.User.Where(u=> u.Id == userId).FirstOrDefaultAsync();
      if(user == null) {
        result.Success=false;
        result.Message=$"there is no user with this id";      
        return result;
      } 

    List<GetChat> chats;

    try 
    {
      var friends =await  _context.User.Where(u=>u.Id == userId).SelectMany(u=>u.Friends.Select(f=>f.Id)).ToListAsync();
      chats =await _context.Chat.Include(c=>c.Members).Where(c=> c.Members.Any(m=>m.UserId == userId)).Select(c=>new GetChat {
      CustomName =c.Name,
      ImageUrl=c.ImageUrl,      
      LastMessage= c.Messages.OrderByDescending(m=>m.SentAt).Select(m=> m.Content).FirstOrDefault(),      
      SentTime = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.SentAt).FirstOrDefault(),
      NumberOfUnSeenMessages=c.Messages.Count(m => m.SeenBy.All(s => s.SeenWith != userId)),    
      ChatId=c.Id,
      ChatType=false   
        }).ToListAsync();
      
    }catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";
      
      return result;
    }

      
      result.Data=chats;
      result.Message="getting chats success";
      return result;       
  }
  
  public async Task<Result<bool>> AddUserImageUrlToDatabase(string imageUrl,string imageId,string userId)
  {
    var result = new Result<bool>();

    if(imageUrl == null){
      result.Success=false;
      result.Message="image url not used";
      
      return result;
    }
   
    try
    {
      var user =await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync();

      if(user == null){
        result.Success=false;
        result.Message="there is no user with this id";
        
        return result;
      }

      user.ImageUrl = imageUrl;
      user.ImageId=imageId;
      await _context.SaveChangesAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      result.Message=$"there is no user with this id {ex}";
      
      return result;
    }

    result.Message="image url added successfully";
    return result;             
  }

    public async Task<Result<bool>> DeleteFriend(CreateIndividualChatDto chatDto)
  {
    var result = new Result<bool>();

    try
    {
      var user =await _context.User.Where(u=>u.Id == chatDto.SenderUserId).FirstOrDefaultAsync();
      var friend =await _context.User.Where(u => u.Id == chatDto.SecondUserId).FirstOrDefaultAsync();

      if(user == null || friend == null){
        result.Success=false;
        result.Message="there is no user with this id";
        
        return result;
      }

      user.Friends.Remove(new Friend{UserId=user.Id,Id = chatDto.SecondUserId,FirstName =friend.FirstName,LastName =friend.LastName});

      await  _context.SaveChangesAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      result.Message=$"there is no user with this id {ex}";
      
      return result;
    }

    result.Message="friend deleted successfully";
    return result;        
  }

  public async Task<Result<bool>> AddGroupMember(string userId,string chatId,string creatorId)
  {
      var result = new Result<bool>();
      try
      {
        var user =await _context.User.Where(u=>u.Id == userId ).FirstOrDefaultAsync();
        var creator =await _context.User.Where(u=>u.Id == creatorId ).FirstOrDefaultAsync();

        if(user == null || creator ==null)
        {            
            result.Success=false;
            result.Message=$"there is no user with this id";          
            return result;;
        }       

        var chat = await _context.Chat.Where(c=>c.Id== chatId).FirstOrDefaultAsync();

        if(chat == null)
        {
            result.Success=false;
            result.Message=$"there is no chat with this id";          
            return result;
        }

        if(creator.Id != chat.GroupCreatorId)
        {
            result.Success=false;
            result.Message=$"just admin can add members";          
            return result;
        }

        var isMemeberOfTheChat =  chat.Members.Exists(cu=>cu.UserId==userId);

        if(isMemeberOfTheChat)
        {
          result.Success=false;          
          result.Message=$"user already member of the group";        
          return result;
        }

        chat.Members.Add(new GroupChatUser{GroupChatId = chat.Id , UserId= userId});

        await _context.SaveChangesAsync();

      }catch(Exception ex)
      {
        result.Success=false;
        _logger.LogInformation($"{ex}");
        result.Message=$"Internal  server error";        
        return result;
      }

      result.Message="member added successfully";
      return result;
  }
}

    
  




