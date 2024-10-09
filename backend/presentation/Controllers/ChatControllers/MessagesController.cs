using Application.ChatsDto;
using Domain.Entities.chatEntities;
using AutoMapper;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace presentation.Controllers.ChatControllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public MessagesController(ApplicationDbContext context, IMapper mapper, ILogger<MessagesController> _logger)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = _logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<MessageDto>> UserChatMessages(string chatId)
    {

        var messages = await _context.Messages
       .Where(m => m.Chat.Id == chatId)
        .Include(m => m.User)
        .OrderBy(m => m.SentAt)
       .ToListAsync();

        var returnedMessages = messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            SentAt = m.SentAt,
            MessageSender = m.User.FirstName,
            IsRead = m.IsRead,
            SeenBy = m.SeenBy.ToList()

        }
        );

        return returnedMessages;

    }


    [HttpPost("api/Messages/Insert")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InsertMessage(InsertMessageDto messageDto)
    {

        var messageInserted = new MessageDto
        {
            Id = Guid.NewGuid().ToString(),
            Content = messageDto.Content
        };

        var user = _context.User.FirstOrDefault(u => u.Id == messageDto.SenderId);
        var chat = _context.Chat.FirstOrDefault(c => c.Id == messageDto.ChatId);

        if (user == null)
        {

            throw new Exception("User not found");
        }

        if (chat == null)
        {

            throw new Exception("Chat not found");
        }

        var message = _mapper.Map<Messages>(messageInserted);
        message.User = user;
        message.Chat = chat;
        message.SentAt = DateTime.UtcNow;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("api/Messages/OpenChat")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> OpenChat(string chatId, string userOpenedId)
    {

        var messages = await _context.Messages
        .Where(m => m.Chat.Id == chatId && !m.IsRead)
        .Include(m => m.User)
        .ToListAsync();

        var seen = messages[0].User.Id == userOpenedId ? false : true;

        if (seen)
        {
            foreach (var m in messages)
            {
                m.IsRead = true;
                m.SeenTime = DateTime.UtcNow;

            }


        }

        await _context.SaveChangesAsync();




        return Ok();

    }



    [HttpPost("api/Messages/OpenGroupChat")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> OpenGroupChat(string chatId, string userId)
    {

        var chat = await _context.Chat
                        .Include(m => m.ChatUser)
                        .FirstOrDefaultAsync(m => m.Id == chatId);

        var chatUsersCount = chat.ChatUser.ToList().Count;

        var user = await _context.User
            .Where(m => m.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return BadRequest("there is no user with this id ");
        }

        var messages = await _context.Messages
        .Where(m => m.Chat.Id == chatId && !m.IsRead)
         .Include(m => m.User)
         .Include(m => m.SeenBy)
         .ToListAsync();

        foreach (var m in messages)
        {
            var seenBy = new SeenBy
            {
                Id = Guid.NewGuid().ToString(),
                SeenWith = user.FirstName,
                SeenTime = DateTime.UtcNow,
                MessagesId = m.Id
            };

            if (m.SeenBy.Any(s => s.SeenWith == seenBy.SeenWith))
            {
                return Ok();
            }
            m.SeenBy.Add(seenBy);

            if (m.SeenBy.Count == chatUsersCount)
            {
                m.IsRead = true;

            }

        }



        await _context.SaveChangesAsync();
        return Ok();

    }

}




