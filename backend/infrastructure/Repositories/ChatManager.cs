using Application.ChatsDto;
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
  public async Task<bool> CreateChat(CreateIndividualChatDto chatDto)
  {
      var newChat = _mapper.Map<IndividualChat>(chatDto);
    
      newChat.Id = Guid.NewGuid().ToString();
      newChat.IndividualChatUser.Add(new IndividualChatUser{UserId = chatDto.SenderUserId,IndividualChatId = newChat.Id,CustomName = chatDto.CustomName ?? ""});
      newChat.IndividualChatUser.Add(new IndividualChatUser{UserId = chatDto.SecondUserId,IndividualChatId = newChat.Id});
      
      var chat =  _context.IndividualChat.Add(newChat);  
      var result =  await _context.SaveChangesAsync();

      return result > 0;
  }
    public async Task<bool> CreateGroupChat(CreateGroupChatDto chatDto)
  {
    var GroupChat = _mapper.Map<GroupChat>(chatDto);
          
    GroupChat.Id = Guid.NewGuid().ToString();

    foreach(var userId in chatDto.GroupChatUsers){
      var existingMember = GroupChat.Members.FirstOrDefault(m => m.UserId == userId);
      if(existingMember == null){
          GroupChat.Members.Add(new GroupChatUser{UserId = userId,GroupChatId =GroupChat.Id});
      }
      
    }    
    
    _context.Chat.Add(GroupChat);

    var result =   await _context.SaveChangesAsync();

    return result > 0;
  }

  public async Task<GetChatResponse> GetIndividualChat(string senderUserId,string secondUserId)
  {
    var Chat =await _context.IndividualChat
                        .Where(c => c.IndividualChatUser.Any(c=>c.UserId == senderUserId) && c.IndividualChatUser.Any(c=> c.UserId == secondUserId) )
                        .FirstOrDefaultAsync();
    
    if(Chat == null){
        return null;
    }

    return new GetChatResponse{Id = Chat.Id};
  }

  public async Task<bool> UpdateChatCustomName(string senderUserId,string customName,string chatId )
  {
    var chatUser = await _context.IndividualChatUser
                                .Where(cu => cu.UserId == senderUserId && cu.IndividualChatId == chatId)
                                .FirstOrDefaultAsync();
                                 
    if(chatUser == null){
          return false;
    }

    chatUser.CustomName = customName;
    var result = await _context.SaveChangesAsync();
    return result > 0;
  }
 
  public async Task<bool> AddImageUrlToDatabase(string imageUrl,string groupChatId)
  {
    if(imageUrl == null){
        return false;
    }

    var GroupChat =await _context.Chat.Where(c=> c.Id == groupChatId).FirstOrDefaultAsync();

    if(GroupChat == null){
      return false;
    }

    GroupChat.ImageUrl = imageUrl;
    var result = await _context.SaveChangesAsync();
     
    return result > 0 ;     
  }

  public async Task<bool> DeleteGroupImageKitImage(string imageId,string groupChatId){
  
    var groupChat = await _context.Chat.Where(c=>c.Id == groupChatId).FirstOrDefaultAsync();

    if(groupChat == null){
    return false;
    }

    groupChat.ImageUrl = "";    

    var result = await _context.SaveChangesAsync(); 

    if(result == 0){
      return false;
    }

    var deleteResult = await _imageKitService.DeleteImage(imageId);

    if(!deleteResult){
      return false;
    }

    return true;
  }

  public async Task<IEnumerable<GetIndividualChat>> GetUserIndividualChats(string userId)
  {
    
    var chats =await _context.IndividualChat.Where(c=> c.IndividualChatUser.Any(u => u.UserId == userId))
    .Select(c => new GetIndividualChat {
    CustomName=c.IndividualChatUser.Where(ic=>ic.UserId == userId).Select(ic=>ic.CustomName).FirstOrDefault(),
    UserImageKitUrl = c.IndividualChatUser.Where(ic=>ic.UserId != userId).Select(ic=>ic.User.ImageUrl ).FirstOrDefault(),
    LastMessage = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.Content).FirstOrDefault(),
    CheckSeen = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.IsRead).FirstOrDefault()
    }
    )
    .ToListAsync();

    return chats;
  }
  
  public async Task<IEnumerable<GetGroupChat>> GetUserGroupChats(string userId)
  {
    
    var friends =await  _context.User.Where(u=>u.Id == userId).SelectMany(u=>u.Friends.Select(f=>f.Id)).ToListAsync();

    var chats =await _context.Chat.Include(c=>c.Members).Where(c=> c.Members.Any(m=>m.UserId == userId)).Select(c=>new GetGroupChat {
      Name =c.Name,
      ImageUrl=c.ImageUrl,
      Members=c.Members.Select(m=>m.User.Id).ToList(),
      LastMessage= c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.Content).FirstOrDefault(),
      CheckSeen = c.Messages.OrderByDescending(m=>m.SentAt).Select(m=>m.IsRead).FirstOrDefault()
        }).ToListAsync();

    var groupNames = new List<string>();

    foreach(var chat in chats)
    {
      string? name = null;
        foreach(var member in chat.Members){
          if(friends.Contains(member)){   
            
              name = await _context.Friends
              .Where(u => userId == u.UserId && u.Id == member)
              .Select(u => u.CustomName)
              .FirstOrDefaultAsync(); 
                  
              name ??= member;     
          }else{   
                name = await _context.User
              .Where(u => member == u.Id)
              .Select(u => u.FirstName)
              .FirstOrDefaultAsync();         
          }
          if (name != null)
          {
              groupNames.Add(name);
          }
        }         
        chat.Members =groupNames;
        groupNames = new List<string>();
    }      
          return chats;
  }

  public async Task<bool> AddUserImageUrlToDatabase(string imageUrl,string userId)
  {
    if(imageUrl == null){
      return false;
    }
    var user =await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync();
    if(user == null){
      return false;
    }

    user.ImageUrl = imageUrl;
    var result = await _context.SaveChangesAsync();

    return result > 0;      
  }

    public async Task<bool> DeleteFriend(CreateIndividualChatDto chatDto)
  {
    
    var user =await _context.User.Where(u=>u.Id == chatDto.SenderUserId).FirstOrDefaultAsync();
    var friend =await _context.User.Where(u => u.Id == chatDto.SecondUserId).FirstOrDefaultAsync();

    if(user == null || friend == null){
      return false;
    }

    user.Friends.Remove(new Friend{UserId=user.Id,Id = chatDto.SecondUserId,FirstName =friend.FirstName,LastName =friend.LastName});

    var result =await  _context.SaveChangesAsync();

    return result > 0;
  }

    
}
    
  




