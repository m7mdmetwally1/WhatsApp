using Application.ChatsDto;
using Application.Interfaces;
using Application.UserDto;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthMangaer _authManager;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IChatManager _chatManager;

    public AuthenticationController(IAuthMangaer _authManager, ILogger<AuthenticationController> logger,IChatManager chatManager)
    {
        this._authManager = _authManager;
        this._logger = logger;
        this._chatManager = chatManager;
    }

    
    [HttpPost]
    public async Task<ActionResult> Register(ApiUserDto userDto)
    {
        var errors = await _authManager.Register(userDto);

        if (errors.Any())
        {
            return BadRequest(new { Errors = errors.Select(e => e.Description) });
        }
        return CreatedAtAction(nameof(Register), new { userDto.PhoneNumber }, "User registered successfully.");
    }

    [HttpGet]
    [Route("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        var errors = await _authManager.ConfirmEmail(userId, token);

        if (errors.Any())
        {
            return BadRequest();
        }
        return Ok("Email Confirmed Successuflly");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(LoginDto login)
    {
        if (login == null)
        {
            return BadRequest("user data is required");
        }

        var result = await _authManager.Login(login);

        if (result == null)
        {
            return BadRequest("Invalid login data");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("VerifyPhoneNumber")]
    public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber, string code)
    {
        if (phoneNumber == null || code == null)
        {
            return BadRequest("please try again later");
        }

        var result = await _authManager.VerifyPhoneNumberCode(phoneNumber, code);

        if (result == false)
        {
            return BadRequest("try again later");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("ResendSms")]
    public async Task<ActionResult> ResendSms(string phoneNumber)
    {
        var result = await _authManager.ResendSms(phoneNumber);

        if (result == false)
        {
            return BadRequest();
        }
        return Ok("Phone Number Confirmed");
    }

    [HttpPost]
    [Route("CheckTwoFactor")]
    public async Task<CheckTwoFactorResponseDto> CheckTwoFactor(CheckTwoFactorDto checkTwoFactorDto)
    {
        var result = await _authManager.CheckTwoFactor(checkTwoFactorDto);

        if (result == false)
        {
            return new CheckTwoFactorResponseDto
            {
                TwoFactorEnabled = false
            };
        }

        return new CheckTwoFactorResponseDto
        {
            TwoFactorEnabled = true
        };
    }

    [HttpPost]
    [Route("VerifyEmail")]
    public async Task<ActionResult> VerifyEmail(string email, string phoneNumber)
    {
        var result = await _authManager.VerifyEmail(email, phoneNumber);

        if (result == false)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("SendTwoFactorCode")]
    public async Task<ActionResult> SendTwoFactorCode(string phoneNumber)
    {
        var result = await _authManager.SendTwoFactorCode(phoneNumber);

        if (result == false)
        {
            return BadRequest("code not sent try again later");
        }
        return Ok("code sent successfully");
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
       
        var imageKitResponse = await  _authManager.UploadImage(uploadImageDto.ImageUrl);
      

         if(string.IsNullOrWhiteSpace(imageKitResponse.FileId) || string.IsNullOrWhiteSpace(imageKitResponse.ImageUrl) ){
            return StatusCode(500, "Failed to upload the image."); 
         }
        
        if(!string.IsNullOrWhiteSpace(uploadImageDto.UserId)){
            
            var addImageUrlToUser = await _chatManager.AddUserImageUrlToDatabase(imageKitResponse.ImageUrl,uploadImageDto.UserId);
        if(!addImageUrlToUser)
            {
            return StatusCode(500,"image uploaded but you cant access it , please try again later");
            }
            return Ok(imageKitResponse); 
        }

        if(!string.IsNullOrWhiteSpace(uploadImageDto.GroupChatId)){
            var addImageUrlToGroupChat = await _chatManager.AddImageUrlToDatabase(imageKitResponse.ImageUrl,uploadImageDto.GroupChatId);       
            if(addImageUrlToGroupChat){
            return Ok(addImageUrlToGroupChat);
            }
            return StatusCode(500,"failed to add the image url to databases");
        }

           return StatusCode(500,"please try again");
    }
    
    [HttpPost]
    [Route("add-friend")]
    public async Task<ActionResult> AddFriend(CreateIndividualChatDto chatDto)
    {
        var success =await _authManager.AddFriend(chatDto);
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
    public async Task<ActionResult> DeleteImage(string? userId,string imageId,string? groupChatId)
    {
        if(!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(groupChatId))
        {
            return BadRequest("the image should be specific for user or Group");
        }

        if(string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(groupChatId) ){
        return BadRequest("should send an id for user or Group");
        }

        if(imageId == null){
        return BadRequest("should provide imageId");
        }
        
        if(!string.IsNullOrWhiteSpace(userId)){
            var checkDeletion = await  _authManager.DeleteUserImageKitImage(imageId,userId); 
            if(checkDeletion){
                return Ok(" user image deleted");
            }
        }
      
      
        if(!string.IsNullOrWhiteSpace(groupChatId)){
            var checkDeletion = await  _chatManager.DeleteGroupImageKitImage(imageId,groupChatId); 
            if(checkDeletion){
                return Ok(" group image deleted");
            }
        }
        return StatusCode(500,"Failed to delete image");
    }
}


