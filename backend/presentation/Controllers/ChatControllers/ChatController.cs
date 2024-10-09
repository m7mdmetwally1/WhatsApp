using Domain.Entities.chatEntities;
using Microsoft.AspNetCore.Mvc;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.ChatsDto;
using System.Diagnostics;

namespace presentation.Controllers.ChatControllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChatController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this._mapper = mapper;
    }


    // [HttpGet("user/{userId}/chats")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<IEnumerable<GetChatsDto>>> GetChats(string userId)
    // {

    //     if (userId == null)
    //     {
    //         return BadRequest("Invalid user ID.");
    //     }

    //     var chats = await _context.Chat
    //     .Where(c => c.ChatUser.Any(cu => cu.UserId == userId))
    //     .Include(c => c.Messages)
    //     .Select(c => new
    //     {
    //         IsGroupChat = c.IsGroupChat,
    //         GroupName = c.Name,
    //         GroupImageUrl = c.IsGroupChat ? c.ImageUrl : null,
    //         LastMessage = c.Messages.OrderByDescending(m => m.SentAt).Select(m => m.Content).FirstOrDefault(),
    //         CustomName = !c.IsGroupChat ? c.ChatUser.Where(cu => cu.UserId == userId).Select(c => c.CustomName) : null,
    //         Users = c.IsGroupChat ? c.ChatUser.Select(c => new { FirstName = c.User.FirstName, CustomName = c.CustomName }) : null
    //     })
    //     .ToListAsync();

    //     if (chats == null || !chats.Any())
    //     {
    //         return NotFound("No chats found for this user.");
    //     }

    //     return Ok(chats);

    // }

    [HttpGet("MyChats")]
    public async Task<ActionResult> GetChats()
    {
        
    }

    [HttpPost("IndividualChat")]
    public async Task<ActionResult> CreateChat()
    {

        //get users ids 
        //check request need individual chat
        // it should be just two ids
        //check if there is chat for the two ids
        // if yes --> update the custom name send 
        //if no --> create the chat with custom name related to the sender and empty for the other 

    }

    [HttpPost("GroupChat")]
    public async Task<ActionResult> CreateGroupChat()
    {
        //validation
        //1- check request needs group chat
        //2- check every userId is valid --> related to valid user
        //3- give the chat Id by ef

        //-create the chat

    }

    [HttpPost("AddGroupChatMember")]
    public async Task<ActionResult> AddGroupChatMember()
    {
        //validation
        //1-check the userid is valid
        //2-authorize the Adder user
        //3- check if user alow easy addation
        //4-if yes --> add it auto 
        //5- if no --> send link to  him to ask him to join
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> NewChat(ChatDto chatDto)
    {


        var existingChat = await _context.Chat
       .Where(c => !c.IsGroupChat)
       .Where(c => c.ChatUser.Count == 2)
       .Where(c => c.ChatUser.Any(cu => cu.User.Id == chatDto.UserIds[0]) &&
                   c.ChatUser.Any(cu => cu.User.Id == chatDto.UserIds[1]))
       .Include(m => m.ChatUser)
       .FirstOrDefaultAsync();


        if (existingChat != null)
        {


            foreach (var c in existingChat.ChatUser)
            {

                if (c.UserId == chatDto.Sender)
                {
                    c.CustomName = chatDto.CustomName;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        if (chatDto.UserIds.Count < 2)
        {
            return BadRequest("You must provide at least two users to create a chat.");
        }

        var chat = _mapper.Map<Chat>(chatDto);
        chat.Id = Guid.NewGuid().ToString();

        if (chatDto.IsGroupChat)
        {

            foreach (var user in chatDto.UserIds)
            {
                chat.ChatUser.Add(new ChatUser { UserId = user, CustomName = chatDto.CustomName });
            }

            chat.Name = chatDto.Name;

        }

        if (!chatDto.IsGroupChat)
        {
            chat.ChatUser.Add(new ChatUser { UserId = chatDto.Sender, CustomName = chatDto.CustomName });
            chat.ChatUser.Add(new ChatUser { UserId = chatDto.otherSender });
        }

        _context.Chat.Add(chat);
        await _context.SaveChangesAsync();

        return Ok();

    }

}

