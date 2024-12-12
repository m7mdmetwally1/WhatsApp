using Application.Common;
using System.Security.Policy;
using Application.Interfaces;
using Application.UserDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using infrastructure.Repositories.SmsService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.chatEntities;
using infrastructure.Data;
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using Application.ChatsDto;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Bcpg;

namespace infrastructure.Repositories;
public class UserManager : IUserManager
{
    private readonly IImageKitService _imageKitService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthManager> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IChatManager _chatManager;

    public UserManager(IImageKitService imageKitService,IConfiguration configuration,ILogger<AuthManager> logger,ApplicationDbContext context,IChatManager chatManager )
  {
    this._imageKitService = imageKitService;
    this._configuration = configuration;
    this._logger = logger;
    this._context = context;
    this._chatManager = chatManager;
  }

  public async Task<Result<ImageKitResponse>> UploadImage(IFormFile imageUrl,string? userId, string? groupChatId)
  {
     var result = new Result<ImageKitResponse>();
     
    if(imageUrl == null){
      result.Success=false;
      result.Message="no image url";      
      return result;
    }

    ImageKitResponse imageKitResponse;

     try
     {     
      
      imageKitResponse  =  await _imageKitService.UploadImage(imageUrl);

       if(!imageKitResponse.Success || imageKitResponse.ImageId==null || imageKitResponse.ImageUrl == null)
       {
        result.Success=false;
        result.Message=$"{imageKitResponse.Message}";
        return result;
       } 
      
      if(userId != null)
      {
        var addingResult = await _chatManager.AddUserImageUrlToDatabase(imageKitResponse.ImageUrl,imageKitResponse.ImageId,userId);      
        if(!addingResult.Success){
           result.Success=false;
           result.Message="failed to add image url to database" ;
           return result;
        } 
      }
              
      if(groupChatId != null){
       var addingResult =  await _chatManager.AddGroupImageUrlToDatabase(imageKitResponse.ImageUrl,imageKitResponse.ImageId,groupChatId);
       if(!addingResult.Success){
           result.Success=false;
           result.Message="failed to add image url to database" ;
           return result;
       }
      }

      }
      catch(Exception ex)
      {      
       result.Success=false;
      _logger.LogInformation($"{ex}");
       result.Message=$"Internal server error";      
       return result;
      }
        
       result.SingleData=imageKitResponse;
       result.Message=$"image uploaded successfully";
       return result;
  }

  public async Task<Result<bool>> DeleteUserImageKitImage(string userId)
  {
    var result = new Result<bool>();

    try
    {
       var user = await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync(); 

      if(user == null){
        result.Success=false;
        result.Message="there is no user with this id";        
        return result;
      }

      if(user.ImageId == null){
        result.Success=false;
        result.Message="there is no image for this user";        
        return result;
      }
      
      using var transaction = await _context.Database.BeginTransactionAsync();

      await _imageKitService.DeleteImage(user.ImageId);

      user.ImageUrl = "";  
      user.ImageId ="";

      await _context.SaveChangesAsync();
      await transaction.CommitAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";      
      return result;
    }
      result.Message=$"image deleted successfully";
      return result;    
  }
  public async Task<Result<bool>> AddFriend(CreateIndividualChatDto chatDto)
  {
    var result = new Result<bool>();

    try
    {
      var user =await _context.User.Include(u=>u.Friends).Where(u=>u.Id == chatDto.SenderUserId).FirstOrDefaultAsync();
      var friend =await _context.User.Where(u => u.Number == chatDto.FriendNumber).FirstOrDefaultAsync();

      if(user == null || friend == null){
        result.Success=false;
        result.Message="there is no user with this id";        
        return result;
      }

      var isFriendAlready =  user.Friends.Any(f =>f.UserId==user.Id && f.Id == friend.Id);
     

      if (!isFriendAlready)
      {
        user.Friends.Add(new Friend{UserId=user.Id,FirstName =friend.FirstName,LastName=friend.LastName,Id=friend.Id,CustomName=chatDto.CustomName??"",ImageUrl=friend.ImageUrl});
      }
      _logger.LogInformation($"{isFriendAlready}");
    
      var chatResult =await _chatManager.GetIndividualChat(chatDto.SenderUserId,friend.Id);

      if(chatResult != null && chatResult.SingleData != null ){
          var chatUpdated = await _chatManager.UpdateChatCustomName(chatDto.SenderUserId,chatDto.CustomName ?? "",chatResult.SingleData.Id); 
          _logger.LogInformation($"{chatUpdated.Message}")  ;         
          if(chatUpdated != null){
            result.Success=true;
            result.Message="chat already exist and your friend custom name updated";            
            return result;
          }
          
          result.Success=false;
          result.Message="chat already exist but your friend custom name doesnt added";          
          return result;          
      }
      chatDto.SecondUserId=friend.Id;
      var chatCreated = await _chatManager.CreateChat(chatDto);
      if(!chatCreated.Success)
      {
        result.Success=false;
        result.Message="cant create chat ,please try again";            
        return result;
      }

      await  _context.SaveChangesAsync();
    }
    catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";      
      return result;
    }

    result.Message=$"you are now friends";
    return result;
  }

   public async Task<Result<bool>> DeleteGroupImageKitImage(string groupId)
  {
    var result = new Result<bool>();

    try
    {
       var group = await _context.Chat.Where(c=>c.Id == groupId).FirstOrDefaultAsync(); 

      if(group == null){
        result.Success=false;
        result.Message="there is no group with this id";        
        return result;
      }

      if(group.ImageId == null){
        result.Success=false;
        result.Message="there is no image for this group";        
        return result;
      }
      
      using var transaction = await _context.Database.BeginTransactionAsync();

      await _imageKitService.DeleteImage(group.ImageId);

      group.ImageUrl = "";  
      group.ImageId ="";

      await _context.SaveChangesAsync();
      await transaction.CommitAsync();

    }catch(Exception ex)
    {
      result.Success=false;
      _logger.LogInformation($"{ex}");
      result.Message=$"Internal server error";      
      return result;
    }
      result.Message=$"image deleted successfully";
      return result;    
  }

  public async Task<Result<GetFriendsDto>> MyFriends(string userId)
  {
    var result = new Result<GetFriendsDto>();
    try
    {
      var friends =  await _context.User.Where(u=>u.Id==userId).Select(u=>u.Friends.Select(f=>new GetFriendsDto {ImageUrl=f.ImageUrl??"",FriendId=f.Id,FriendCustomName=f.CustomName??f.FirstName})).FirstOrDefaultAsync();
      
      if(friends == null)
      {
        result.Message="you have no friends";
        return result;
      }

      result.Data=friends;

    }catch(Exception err)
    {
      _logger.LogInformation($"{err}");
      result.Message="internal server error";
      return result;
    }
    return result;
  }
 
}






