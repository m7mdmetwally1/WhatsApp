using Domain.Entities.chatEntities;
using Microsoft.AspNetCore.Mvc;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.ChatsDto;
using System.Diagnostics;
using Application.Interfaces;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

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
    public async Task<ActionResult> GetChats(string userId)
    {
        if(userId == null){
            return BadRequest("should provide userId");
        }
        var user =await _userManager.FindByIdAsync(userId);
        if(user ==null){
            return BadRequest("user not valid");
        }
        var individualChats = await _chatManager.GetUserIndividualChats(userId);

        var groupChats = await _chatManager.GetUserGroupChats(userId);
        
        _logger.LogInformation($"{groupChats}");
        var result = new { individualChats, groupChats };

        if(individualChats != null && groupChats != null){
            return Ok(result);
        }
        return StatusCode(500,"error , please try again");
    }

    [HttpPost("CreateGroupChat")]
    public async Task<ActionResult> CreateGroupChat(CreateGroupChatDto createGroupChatDto)
    {
       
        if(createGroupChatDto.ChatType != ChatType.Group){
            return BadRequest("you should specify chat type as Group");
        }

        foreach(var userId in createGroupChatDto.GroupChatUsers){
            var user =await _userManager.FindByIdAsync(userId);
            if(user == null){
                return BadRequest("there is an valid user");
            }
        }  
          
        var GroupChatCreated =await _chatManager.CreateGroupChat(createGroupChatDto);
        if(GroupChatCreated){
            return Ok("created successfully");
        }

        return StatusCode(500,"failed to create the chat please try again");
       
    }

}


    // [HttpPost("AddGroupChatMember")]
    // public async Task<ActionResult> AddGroupChatMember()
    // {
    //     //validation
    //     //1-check the userid is valid
    //     //2-authorize the Adder user
    //     //3- check if user alow easy addation
    //     //4-if yes --> add it auto 
    //     //5- if no --> send link to  him to ask him to join
    // }


    
