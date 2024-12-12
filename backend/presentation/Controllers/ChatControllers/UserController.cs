using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Application.ChatsDto;
using Application.Interfaces;
using Application.UserDto;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.AspNetCore.Authorization;

namespace presentation.Controllers.ChatControllers;

public class UserController :ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthMangaer _authManager;
    private readonly ILogger<UserController> _logger;
    private readonly IChatManager _chatManager;
    private readonly IUserManager _userManager;

    public UserController(ApplicationDbContext context,IAuthMangaer _authManager, ILogger<UserController> logger,IChatManager chatManager,IUserManager userManager)
    {
        this._context = context;
        this._authManager = _authManager;
        this._logger = logger;
        this._chatManager = chatManager;
        this._userManager = userManager;
    }

    [HttpPost]
    [Route("add-friend")]
    //  [Authorize]
    public async Task<ActionResult> AddFriend(CreateIndividualChatDto chatDto)
    {   

        var result =await _userManager.AddFriend(chatDto);
        _logger.LogInformation($"{chatDto}");
         if(!result.Success)
            {
                return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
            }  
             
        return Ok(new {Success=result.Success,Message =result.Message});
                
    }

    [HttpPost]
    [Route("Delete-image")]
     [Authorize]
    public async Task<ActionResult> DeleteImage(string? userId,string? groupChatId)
    {
        if(string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(groupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }
        if(!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(groupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }
        var result = new Result<bool>();
            
        if(!string.IsNullOrWhiteSpace(userId)){           
           
             result = await  _userManager.DeleteUserImageKitImage(userId);
           
             if(!result.Success)
            {
                return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
            }                          
        }
            
        if(!string.IsNullOrWhiteSpace(groupChatId)){

            result = await  _userManager.DeleteGroupImageKitImage(groupChatId);

            if(!result.Success)
            {
                return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
            }          
             
        } 

        return Ok(new {Success=result.Success,Message=result.Message});          

    }

    [HttpPost]
    [Route("Upload-Image")]
    //  [Authorize]
    public async Task<ActionResult> UploadImage(UploadImageDto uploadImageDto)
    {
        _logger.LogInformation($"{uploadImageDto.ImageUrl} test");
        _logger.LogInformation($"{uploadImageDto.UserId} test");
        if(!string.IsNullOrWhiteSpace(uploadImageDto.UserId) && !string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }
      
        if(string.IsNullOrWhiteSpace(uploadImageDto.UserId) && string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId) ){
        return BadRequest("should send an id for user or Group");
        }
      
        if(uploadImageDto.ImageUrl == null){
        return BadRequest("imageUrl can not be null");
        }        ;
     
        var result = await  _userManager.UploadImage(uploadImageDto.ImageUrl,uploadImageDto.UserId,uploadImageDto.GroupChatId);

         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }
                   
        return Ok(new {Success=true,Message=result.Message,ImageUrl=result.SingleData.ImageUrl??""});
    } 

    [HttpPost]
    [Route("my-friends")]
     [Authorize]
    public async Task<ActionResult> MyFriends(string userId)
    {
        if(userId== null) return BadRequest("no userId sent");

        var result = await _userManager.MyFriends(userId);

        if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }

        return Ok(new {Success=true,Message=result.Message,Data=result.Data});
        
    }

}

