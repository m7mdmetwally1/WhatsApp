using Application.ChatsDto;
using Domain.Entities.chatEntities;
using AutoMapper;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Application.Interfaces;
using Application.Common;

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
    public async Task<ActionResult> InsertMessage(InsertIndividualMessageDto message)
    {
       
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

 








