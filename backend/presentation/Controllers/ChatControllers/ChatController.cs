using Domain.Entities.chatEntities;
using Application.ChatsDto;
using Microsoft.AspNetCore.Mvc;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Diagnostics;
using Application.Interfaces;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Application.Common;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.AspNetCore.Authorization;

namespace presentation.Controllers.ChatControllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IChatManager _chatManager;
    private readonly IImageKitService _imageKitService;
    private readonly IAuthMangaer _authManager;
    private readonly ILogger<ChatController> _logger;

    public ChatController(ApplicationDbContext context, IMapper mapper,UserManager<ApiUser> userManager,IChatManager chatManager,IImageKitService imageKitService,IAuthMangaer authManager,ILogger<ChatController> logger)
    {
        this._context = context;
        this._mapper = mapper;
        this._userManager = userManager;
        this._chatManager = chatManager;
        this._imageKitService = imageKitService;
        this._authManager = authManager;
        this._logger = logger;
    }
    
    [HttpGet("MyChats")]
    [Authorize]
    public async Task<ActionResult> GetChats(string userId)
    {
    
        if(userId == null){
            return BadRequest("should provide userId");
        }
            
        var individualChatsResult = await _chatManager.GetUserIndividualChats(userId);

       if(!individualChatsResult.Success)
            {
                return StatusCode(500,new {Success=false,ErrorMessage=$"{individualChatsResult.Message}"}); 
            }  
        
        var groupChatsResults = await _chatManager.GetUserGroupChats(userId);
        
        if(!groupChatsResults.Success)
            {
                return StatusCode(500,new {Success=false,ErrorMessage=$"{groupChatsResults.Message}"}); 
            }  
        var allChats = individualChatsResult.Data.Concat(groupChatsResults.Data);              

        return Ok(new { Success= individualChatsResult.Success,Data = allChats,});
    }

    [HttpPost("CreateGroupChat")]
     [Authorize]
    public async Task<ActionResult> CreateGroupChat(CreateGroupChatDto createGroupChatDto)
    {                         
        
        var result =await _chatManager.CreateGroupChat(createGroupChatDto);
               
        if(!result.Success)
        {
            return BadRequest(new {Success=false,ErrorMessage=$"{result.Message}"});
        }

        return Ok(new {Success=true,Message=result.Message}); 
             
    }

    [HttpPost]
    [Route("AddGroupMember")]
     [Authorize]
    public async Task<ActionResult> AddGroupMember(string userId,string chatId,string creatorId)
    {                   
        var result = await _chatManager.AddGroupMember(userId,chatId,creatorId);
               
         if(!result.Success)
        {
            return BadRequest(new {Success=false,ErrorMessage=$"{result.Message}"});
        }

        return Ok(new {Success=true,Message=result.Message});                     
    }

}





    
