using Microsoft.AspNetCore.Mvc;
using Application.ChatsDto;
using Application.Interfaces;
using Application.UserDto;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult> AddFriend(CreateIndividualChatDto chatDto)
    {
        var success =await _userManager.AddFriend(chatDto);
        if(!success){
            return StatusCode(500,"failed to add friend");
        }
        var chat =await _chatManager.GetIndividualChat(chatDto.SenderUserId,chatDto.SecondUserId);
        
        if(chat != null ){
            var chatUpdated = await _chatManager.UpdateChatCustomName(chatDto.SenderUserId,chatDto.CustomName ?? "",chat.Id);            
            if(chatUpdated) return Ok("chat already exist and your friend custom name updated");
            return Ok("chat already exist but your friend custom name doesnt added");
        }
        
        var chatCreated = await _chatManager.CreateChat(chatDto);
        if(!chatCreated){
            var deleteFriend = await _chatManager.DeleteFriend(chatDto);
            return StatusCode(500,"failed to create chat");
        }

        return Ok();
    }

    [HttpPost]
    [Route("Delete-image")]
    public async Task<ActionResult> DeleteImage(string? userId,string? groupChatId)
    {
        if(string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(groupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }
        
        if(!string.IsNullOrWhiteSpace(userId)){
            var user =await _context.User.Where(u=>u.Id == userId).FirstOrDefaultAsync();

            if(user ==null)
            {
                return BadRequest("user not exist");
            }

            if(user.ImageId ==null)
            {
                return StatusCode(500,"image already deleted");
            }
           
            var checkDeletion = await  _userManager.DeleteUserImageKitImage(user.ImageId,userId);

            if(checkDeletion){
                return Ok(" user image deleted");
            }
            if(!checkDeletion){
                return StatusCode(500,"failed to delete image please try again");
            }
        }
            
        if(!string.IsNullOrWhiteSpace(groupChatId)){
            var groupChat = await _context.Chat.Where(c=>c.Id == groupChatId).FirstOrDefaultAsync();

            if(groupChat ==null)
            {
                return BadRequest("user not exist");
            }

            if(groupChat.ImageId ==null)
            {
                return StatusCode(500,"image already deleted");
            }

            var checkDeletion = await  _chatManager.DeleteGroupImageKitImage(groupChat.ImageId,groupChatId); 

            if(checkDeletion){
                return Ok(" group image deleted");
            }
        }
        return StatusCode(500,"Failed to delete image");
    }

    [HttpPost]
    [Route("Upload-Image")]
    public async Task<ActionResult> UploadImage(UploadImageDto uploadImageDto)
    {
        if(!string.IsNullOrWhiteSpace(uploadImageDto.UserId) && !string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }
      
        if(string.IsNullOrWhiteSpace(uploadImageDto.UserId) && string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId) ){
        return BadRequest("should send an id for user or Group");
        }
      
        if(uploadImageDto.ImageUrl == null){
        return BadRequest("imageUrl can not be null");
        }
       
        var imageKitResponse = await  _userManager.UploadImage(uploadImageDto.ImageUrl);
      

         if(string.IsNullOrWhiteSpace(imageKitResponse.ImageId) || string.IsNullOrWhiteSpace(imageKitResponse.ImageUrl) ){
            return StatusCode(500, "Failed to upload the image."); 
         }
        
        if(!string.IsNullOrWhiteSpace(uploadImageDto.UserId)){
            
            var addImageUrlToUser = await _chatManager.AddUserImageUrlToDatabase(imageKitResponse.ImageUrl,imageKitResponse.ImageId,uploadImageDto.UserId);
        if(!addImageUrlToUser)
            {
            return StatusCode(500,"image uploaded but you cant access it , please try again later");
            }
            return Ok(imageKitResponse); 
        }

        if(!string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId)){
            var addImageUrlToGroupChat = await _chatManager.AddGroupImageUrlToDatabase(imageKitResponse.ImageUrl,imageKitResponse.ImageId,uploadImageDto.GroupChatId);       
            if(addImageUrlToGroupChat){
            return Ok(addImageUrlToGroupChat);
            }
            return StatusCode(500,"failed to add the image url to databases");
        }

           return StatusCode(500,"please try again");
    }  

}