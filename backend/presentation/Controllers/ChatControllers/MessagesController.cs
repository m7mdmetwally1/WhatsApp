using Application.ChatsDto;
using Domain.Entities.chatEntities;
using AutoMapper;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Application.Interfaces;
using Application.Common;
using Microsoft.AspNetCore.Authorization;

namespace presentation.Controllers.ChatControllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IMessagesManager _messagesManager;

    public MessagesController(ApplicationDbContext context, IMapper mapper, ILogger<MessagesController> _logger,IMessagesManager messagesManager)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = _logger;
        this._messagesManager = messagesManager;
    }
    
    [HttpGet]
    [Route("IndividualChat")]
     [Authorize]
    public async Task<ActionResult> IndividualChatMessages(string chatId,string userId)
    {         
       
        var result = await _messagesManager.IndividualChatMessages(userId, chatId);

         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }        

        return Ok(new {Success=result.Success,Data=result.Data});       
    }
    
    [HttpGet]
    [Route("GroupChat")]
     [Authorize]
    public async  Task<ActionResult> GroupChatMessages(string chatId,string userId)
    {               
        var result = await _messagesManager.GroupChatMessages(userId, chatId);

        if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }        

        return Ok(new {Success=result.Success,Data=result.Data});
    }
    
    [HttpPost]
    [Route("InsertMessage/IndividualChat")]
     [Authorize]
    public async Task<ActionResult> InsertMessage([FromBody] InsertIndividualMessageDto message)
    {
        _logger.LogInformation($"{message.UserId} before inter");
       
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       var result =  await _messagesManager.InsertIndividualMessage(message);       

        if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }        

        return Ok(new {Success=result.Success,Message=result.Message}); 
    }

    [HttpPost]
    [Route("InsertMessage/GroupChat")]
     [Authorize]
    public async Task<ActionResult> InsertMessage(InsertGroupMessageDto message)
    {    
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
               
        var result =  await _messagesManager.InsertGroupMessage(message);

         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }        

        return Ok(new {Success=result.Success,Message=result.Message});
    }

    [HttpPost]
    [Route("Open/IndividualChat")]
     [Authorize]
    public async Task<ActionResult> OpenIndividualChat(string userId,string chatId)
    {
       if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _messagesManager.OpenIndividualChat(userId,chatId);
                
         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }       

        return Ok(new {Success=result.Success,Message=result.Message});

    }

    [HttpPost]
    [Route("Open/GroupChat")]
     [Authorize]
    public async Task<ActionResult> OpenGroupChat(string userId,string chatId)
    {
        if(userId== null){
            return BadRequest("user Id is required");
        }

        if(chatId == null){
            return BadRequest("chatId is required");
        }    
        
        var result = await _messagesManager.OpenGroupChat(userId,chatId);
                
         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }        

        return Ok(new {Success=result.Success,Message=result.Message});
    }

    [HttpPost]
    [Route("ChangeContent")]
     [Authorize]
    public async Task<ActionResult> ChangeContent(string userId,string chatId,string messageId,string newContent)
    {
        if(userId== null){
            return BadRequest("user Id is required");
        }

        if(chatId == null){
            return BadRequest("chatId is required");
        }

        var result = await _messagesManager.ChangeIndividualChatMessageContent(userId,chatId,messageId,newContent);
                             
         if(!result.Success)
        {
            return StatusCode(500,new {Success=false,ErrorMessage=$"{result.Message}"}); 
        }            

        return Ok(new {Success=result.Success,Message=result.Message});
    }    
}

 








