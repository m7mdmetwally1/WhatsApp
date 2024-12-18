using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Application.UserDto;
using Application.ChatsDto;


namespace Application.Interfaces;

public interface IAuthMangaer
{
     Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
     Task<IEnumerable<IdentityError>> ConfirmEmail(string userId, string token);
     Task<bool> VerifyPhoneNumberCode(string id,string code);
     Task<bool> ResendSms (string phoneNumber);
     Task<bool> EnableDisableTwoFactor (string userId);
     Task<bool> VerifyEmail (string email,string phoneNumber);
     Task<bool> SendTwoFactorCode (string phoneNumber);
     Task<AuthResponseDto> VerifyRefreshToken (string refreshToken);
     Task<AuthResponseDto> Login(LoginDto loginDto);      
}

