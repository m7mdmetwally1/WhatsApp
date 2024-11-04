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

        if(result.StatusCode == 500)
        {
            return StatusCode(500,new Result<GetIndividualMessagesDto>{Success=false,ErrorMessage=$"{result.ErrorMessage}"}); 
        }
        if(result.StatusCode == 400)
        {
            return BadRequest(new Result<GetIndividualMessagesDto>{Success=false,ErrorMessage=$"{result.ErrorMessage}"});
        }

        return Ok(result);       
    }

    [HttpGet]
    [Route("GroupChat")]
    public async  Task<ActionResult> GroupChatMessages(string chatId,string userId)
    {               
        var result = await _messagesManager.GroupChatMessages(userId, chatId);

        if(result.StatusCode == 500)
        {
            return StatusCode(500,new Result<GetIndividualMessagesDto>{Success=false,ErrorMessage=$"{result.ErrorMessage}"}); 
        }
        if(result.StatusCode == 400)
        {
            return BadRequest(new Result<GetIndividualMessagesDto>{Success=false,ErrorMessage=$"{result.ErrorMessage}"});
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("InsertMessage/IndividualChat")]
    public async Task<ActionResult> InsertMessage(InsertIndividualMessageDto message)
    {
       
        if(ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       var result =  await _messagesManager.InsertIndividualMessage(message);

        if(result.StatusCode == 400)
        {
            return BadRequest($"{result.Message}");
        }

        if(result.StatusCode == 200)
        {
            return Ok($"{result.Message}");
        }

        return BadRequest($"{result.Message}"); 
    }

    [HttpPost]
    [Route("InsertMessage/GroupChat")]
    public async Task<ActionResult> InsertMessage(InsertGroupMessageDto message)
    {
        if(message.UserId == null){
            return BadRequest("user Id is required");
        }

        if(message.ChatId == null){
            return BadRequest("chatId is required");
        }
        
       var result =  await _messagesManager.InsertGroupMessage(message);

        if(result.StatusCode == 400)
        {
            return BadRequest($"{result.Message}");
        }

        if(result.StatusCode == 200)
        {
            return Ok($"{result.Message}");
        }

            return BadRequest($"{result.Message}");
    }

    [HttpPost]
    [Route("Open/IndividualChat")]
    public async Task<ActionResult> OpenIndividualChat(string userId,string chatId)
    {
        if(userId== null){
            return BadRequest("user Id is required");
        }

        if(chatId == null){
            return BadRequest("chatId is required");
        }

       var result = await _messagesManager.OpenIndividualChat(userId,chatId);

        
        if(result.StatusCode == 400)
        {
            return BadRequest($"{result.Message}");
        }

        if(result.StatusCode == 200)
        {
            return BadRequest($"{result.Message}");
        }

            return BadRequest($"{result.Message}");

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

        
        if(result.StatusCode == 400)
        {
            return BadRequest($"{result.Message}");
        }

        if(result.StatusCode == 200)
        {
            return Ok($"{result.Message}");
        }

            return BadRequest($"{result.Message}");

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
                             
        try
        {
           var result = await _messagesManager.ChangeIndividualChatMessageContent(userId,chatId,messageId,newContent);
            if(result)  return Ok();
            return BadRequest(new { StatusCode = 400, Message = "An unexpected error occurred." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
         catch (KeyNotFoundException ex)
        {
            return NotFound(new { StatusCode = 404, Message = ex.Message });
        }
         catch (Exception ex)
        {       
            return StatusCode(500, new { StatusCode = 500, Message = "An unexpected error occurred." });
        }
    }    
}

 








