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



    public AuthenticationController(IAuthMangaer _authManager, ILogger<AuthenticationController> logger)
    {
        this._authManager = _authManager;
        this._logger = logger;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

