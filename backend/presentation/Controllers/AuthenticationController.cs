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
        
        if(!ModelState.IsValid)
        {                       
            return BadRequest("Invalid request");
        }        

        var errors = await _authManager.Register(userDto);        
        if (errors.Any())
        {
            return BadRequest(new { Errors = errors.Select(e => e.Description) });
        }
        return CreatedAtAction(nameof(Register), new { number= userDto.PhoneNumber,message = "User registered successfully."});
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
        

        if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
        {
            return BadRequest(new {Error= result.ErrorMessage});
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("VerifyRefreshToken")]
    public async Task<ActionResult> VerifyRefreshToken(string refreshToken)
    {
         if (refreshToken == null)
        {
            return BadRequest("no refresh token send");
        }
    
        var result = await _authManager.VerifyRefreshToken(refreshToken);

        if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
        {
            return BadRequest("Invalid refresh token ");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("VerifyPhoneNumber")]
    public async Task<ActionResult> VerifyPhoneNumber(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return BadRequest(new {error="invalid data"});
        }

        var result = await _authManager.VerifyPhoneNumberCode(userId, code);

        if (result == false)
        {
            return BadRequest(new {error="invalid intered code "});
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
    [Route("EnableDisableTwoFactor")]
    public async Task<ActionResult> EnableDisableTwoFactor(string userId)
    {
        var result = await _authManager.EnableDisableTwoFactor(userId);
       
       return Ok(new {TwoFactorEnabled=result});
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

}


