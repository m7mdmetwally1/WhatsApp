
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

namespace infrastructure.Repositories;

public class UserManager : IUserManager
{
  private readonly IImageKitService _imageKitService;
  private readonly IConfiguration _configuration;
  private readonly IMapper _mapper;
  private readonly ILogger<AuthManager> _logger;
  private readonly ApplicationDbContext _context;

  public UserManager(IImageKitService imageKitService,IConfiguration configuration,IMapper mapper,ILogger<AuthManager> logger,ApplicationDbContext context )
  {
    this._imageKitService = imageKitService;
    this._configuration = configuration;
    this._logger = logger;
    this._context = context;
  }

  public async Task<ImageKitResponse> UploadImage(IFormFile imageUrl)
  {
    if(imageUrl == null){
      return null;
    }
      var imageKitResponse =  await _imageKitService.UploadImage(imageUrl);
      if(imageKitResponse == null){
        return null;
      }
      return imageKitResponse;
  }

  public async Task<bool> DeleteUserImageKitImage(string imageId,string userId){
    
    var user = await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync(); 

    if(user == null){
    return false;
    }

    user.ImageUrl = "";  
    user.ImageId ="";

    var result = await _context.SaveChangesAsync();
    
    if(result == 0 ){
      return false;
    }

    var deleteResult = await _imageKitService.DeleteImage(imageId);

    if(!deleteResult){
      return false;
    }

    return true;
  }

  public async Task<bool> AddFriend(CreateIndividualChatDto chatDto)
  {
    
    var user =await _context.User.Where(u=>u.Id == chatDto.SenderUserId).FirstOrDefaultAsync();
    var friend =await _context.User.Where(u => u.Id == chatDto.SecondUserId).FirstOrDefaultAsync();

    if(user == null || friend == null){
      return false;
    }

    var isFriendAlready = user.Friends.Any(f => f.Id == friend.Id);

    if (isFriendAlready)
    {
        return true;
    }
  
    user.Friends.Add(new Friend{UserId=user.Id,FirstName =friend.FirstName,LastName=friend.LastName,Id=friend.Id,CustomName=chatDto.CustomName??""});

    var result =await  _context.SaveChangesAsync();

    return result > 0;
  }

 
}






