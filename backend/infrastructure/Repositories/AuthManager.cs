
using System.Security.Policy;
using Application.Interfaces;
using Application.UserDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using infrastructure.Repositories.SmsService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.chatEntities;
using infrastructure.Data;


namespace infrastructure.Repositories;

public class AuthManager : IAuthMangaer
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IMapper _mapper;
  
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly ILogger<AuthManager> _logger;
    private readonly ApplicationDbContext _context;
  
    private ApiUser _user;


    public AuthManager(IConfiguration configuration,UserManager<ApiUser> userManager,IMapper mapper,SignInManager<ApiUser> signInManager,IEmailSender emailSender,ISmsSender smsSender,ILogger<AuthManager> logger,ApplicationDbContext context )
    {
        this._configuration = configuration;
        this._userManager = userManager;
        this._mapper = mapper;
    
        this._signInManager = signInManager;
        this._emailSender = emailSender;
        this._smsSender = smsSender;
        this._logger = logger;
        this._context = context;
    }


    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto){

      if (userDto == null)
    {
        return new List<IdentityError> { new IdentityError { Description = "User data is required." } };
    }

        
         _user = _mapper.Map<ApiUser>(userDto);
        _user.UserName = userDto.PhoneNumber;

        
        

        var result = await _userManager.CreateAsync(_user,userDto.Password);

        

         if(result.Succeeded){
           

          var code = await _userManager.GenerateChangePhoneNumberTokenAsync(_user, userDto.PhoneNumber);

             

              // await _smsSender.SendSmsAsync($"{userDto.PhoneNumber}",$"your verification code is {code}"); 

              var chatUser = new User
    {      
        Id = _user.Id,
        FirstName=_user.FirstName,
        LastName=_user.LastName
    };
  
          _context.User.Add(chatUser);
        await _context.SaveChangesAsync();
              
         }



         return result.Errors;

    }

    
      public async Task<IEnumerable<IdentityError>> ConfirmEmail(string userId, string token)
        {
           var  user = await _userManager.FindByIdAsync(userId);


           
          
            _logger.LogInformation($"{token} token recieved");
             

            if(user == null){
              return null;
            }
                
                token = token.Replace(" ", "+");


                var result = await _userManager.ConfirmEmailAsync(user, token);
             
        _logger.LogInformation($"{result} result of query");

             if(!result.Succeeded){



                return result.Errors;
             }


              _logger.LogInformation($"Email confirmed successfully for user with ID {userId}");

             return   result.Errors;
            
        }


    public async Task<AuthResponseDto> Login(LoginDto login){


      if(login == null ){
        return null;
      }
      
         var user = await _userManager.FindByNameAsync(login.PhoneNumber);

        
      _logger.LogInformation($"{user.UserName}");

        if (user == null) {
      return null;
    }

       bool isValidUser = await _userManager.CheckPasswordAsync(user, login.Password);



       if(isValidUser) {
        
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

       var isEmailConfirmed =  await _userManager.IsEmailConfirmedAsync(user);

          // var token = await GenerateJWTToken(user);
          var token = await GenerateJWTToken();

           return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                RefreshToken="",
                PhoneNumber=user.PhoneNumber,
             IsTwoFactorEnabled=isTwoFactorEnabled,
             IsEmailConfirmed=isEmailConfirmed
            };
        }

        return null;

      

    } 

         private async  Task<string> GenerateJWTToken()
        {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_configuration["jwtSettings:Issuer"],
              _configuration["jwtSettings:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
              var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

                 return token;

        }

        



 public async Task<bool> VerifyPhoneNumberCode(string phoneNumber, string code)
{

  var user = await _userManager.FindByNameAsync(phoneNumber);

    if (user == null)
    {
        return false;
    }

      var isValidCode = await _userManager.VerifyChangePhoneNumberTokenAsync(user, code, user.PhoneNumber);

      

  if (isValidCode)
{
    user.PhoneNumberConfirmed = true;  
    var result = await _userManager.UpdateAsync(user);  

    if (result.Succeeded)
    {
        return true;
    }
}
    return false;

}


 public async Task<bool> ResendSms(string  phoneNumber)
{

  var _user = await _userManager.FindByNameAsync(phoneNumber);

   

    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(_user,phoneNumber);



    try{
      await _smsSender.SendSmsAsync($"{phoneNumber}",$"your new  verification code is {code}"); 
      return true;

    }catch(Exception ex){
      return false;
    }


}


 public async Task<bool> CheckTwoFactor(CheckTwoFactorDto checkTwoFactorDto)
{

  var _user = await _userManager.FindByNameAsync(checkTwoFactorDto.PhoneNumber);

    
   var enabled = await _userManager.GetTwoFactorEnabledAsync(_user);

    if(enabled){
      return true;
    }

await _userManager.SetTwoFactorEnabledAsync(_user, true);

  
return false;
   


}



 public async Task<bool> VerifyEmail(string email,string phoneNumber )
{

  var _user = await _userManager.FindByNameAsync(phoneNumber);

   var isConfirmed =  await _userManager.IsEmailConfirmedAsync(_user);

  if(isConfirmed  || _user == null ){
    return  false;
  }

   var token = await _userManager.GenerateEmailConfirmationTokenAsync(_user);

   var confirmationLink = $"http://localhost:5233/api/Authentication/confirm-email?userId={_user.Id}&token={token}";

  
    await _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your email by <a href='{confirmationLink}'>clicking here</a>;.");

return true;

}



public async Task<bool> SendTwoFactorCode(string phoneNumber){

  

  var _user = await _userManager.FindByNameAsync(phoneNumber);

  if(_user == null ){
    return false;
  }

  var code = await _userManager.GenerateChangePhoneNumberTokenAsync(_user, phoneNumber);

try{
 await _smsSender.SendSmsAsync($"{phoneNumber}",$"your verification code is {code}"); 
 return true;

}catch(Exception ex){

  return false;

}



}

}





