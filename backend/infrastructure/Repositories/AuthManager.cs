
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
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using Application.ChatsDto;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Reflection.Emit;

namespace infrastructure.Repositories;

public class AuthManager : IAuthMangaer
{
  private readonly IImageKitService _imageKitService;
  private readonly IConfiguration _configuration;
  private readonly UserManager<ApiUser> _userManager;
  private  readonly IMapper _mapper;
  private readonly SignInManager<ApiUser> _signInManager;
  private readonly IEmailSender _emailSender;
  private readonly ISmsSender _smsSender;
  private readonly ILogger<AuthManager> _logger;
  private readonly ApplicationDbContext _context;

  private ApiUser? _user;

  public AuthManager(IImageKitService imageKitService,IConfiguration configuration,UserManager<ApiUser> userManager,IMapper mapper,SignInManager<ApiUser> signInManager,IEmailSender emailSender,ISmsSender smsSender,ILogger<AuthManager> logger,ApplicationDbContext context )
  {
    this._imageKitService = imageKitService;
    this._configuration = configuration;
    this._userManager = userManager;
    this._mapper = _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    this._signInManager = signInManager;
    this._emailSender = emailSender;
    this._smsSender = smsSender;
    this._logger = logger;
    this._context = context;
  }

  public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
  {    
    
    _user = _mapper.Map<ApiUser>(userDto);
      _user.UserName = userDto.PhoneNumber;
    
    var result = await _userManager.CreateAsync(_user,userDto.Password);     
    _logger.LogInformation($"{result.Succeeded}"); 
    if(result.Succeeded){
         
        _logger.LogInformation($"{userDto.PhoneNumber}");
       

        var chatUser = new User
        {      
            Id = _user.Id,
            FirstName=_user.FirstName,
            LastName=_user.LastName??"",
            Number=_user.PhoneNumber ??"000"
        };
          
        _context.User.Add(chatUser);
        await _context.SaveChangesAsync();              
    }

    return result.Errors;
  }
    
  public async Task<IEnumerable<IdentityError>> ConfirmEmail(string userId, string token)
  {
    var  user = await _userManager.FindByIdAsync(userId);
             
    if(user == null){
      var error = new IdentityError
        {
            Code = "UserNotFound",
            Description = "There is no user with the specified ID."
        };
      return new List<IdentityError> { error };
    }
                
    token = token.Replace(" ", "+");

    var result = await _userManager.ConfirmEmailAsync(user, token);             

    if(!result.Succeeded){
      return result.Errors;
    }

    return   result.Errors;            
  }

  public async Task<AuthResponseDto> Login(LoginDto login)
  {
    if(login == null ){
      return new AuthResponseDto
      {
          ErrorMessage= "Invalid data"
      };
    }
      
    var user = await _userManager.FindByNameAsync(login.PhoneNumber);            

    if (user == null || user.PhoneNumber ==null)
    {
      return new AuthResponseDto
      {
          ErrorMessage= "there is no user with this number"
      };
    }
    
    bool isValidUser = await _userManager.CheckPasswordAsync(user, login.Password);

    if(isValidUser)
    {        
      var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
      var imageUrl = await _context.User.Where(u=>u.Id==user.Id).Select(u=>u.ImageUrl).FirstOrDefaultAsync();

      var isEmailConfirmed =  await _userManager.IsEmailConfirmedAsync(user);

      var token =  GenerateJWTToken();
      var RefreshToken = GenerateRefreshToken(user.PhoneNumber);
      
      // if(isTwoFactorEnabled)
      // {
      //   var userNumber=$"+2{user.PhoneNumber}";         
      //   var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
      //   var result =    await _smsSender.SendSmsAsync($"{userNumber}",$"your verification code is {code}");                 
      //   isTwoFactorEnabled = result ? true : false;
      // }
        
      return new AuthResponseDto
      {
          Token = token,
          UserId = user.Id,
          RefreshToken=RefreshToken, 
          PhoneNumber=user.PhoneNumber,
          ImageUrl=imageUrl,
        IsTwoFactorEnabled=isTwoFactorEnabled,
        IsEmailConfirmed=isEmailConfirmed
      };
    }

      return  new AuthResponseDto
      {
        ErrorMessage= "password not correct"
      };
  } 
  private string GenerateJWTToken()
  {
    var jwtKey = _configuration["jwtSettings:Key"] ?? "my-ultra-secure-and-ultra-long-secret";
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var Sectoken = new JwtSecurityToken(_configuration["jwtSettings:Issuer"],
                                        _configuration["jwtSettings:Audience"],
                                        null,
                                        expires: DateTime.Now.AddMinutes(120),
                                        signingCredentials: credentials);
                                        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

    return token;

  }

  public async Task<bool> VerifyPhoneNumberCode(string userId, string code)
  {

    var user = await _userManager.FindByIdAsync(userId);

    if (user == null || user.PhoneNumber == null)
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

    if(_user == null)
    {
      return false;
    }

    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(_user,phoneNumber);

    try{
      await _smsSender.SendSmsAsync($"{phoneNumber}",$"your new  verification code is {code}"); 
      return true;

    }catch(Exception ex){
      _logger.LogInformation($"{ex}");
      return false;
    }

  }

  public async Task<bool> EnableDisableTwoFactor(string userId)
  {

    var _user = await _userManager.FindByIdAsync(userId);

    if(_user == null)
    {
      return false;
    }

    if(!_user.TwoFactorEnabled)
    {
       var result= await _userManager.SetTwoFactorEnabledAsync(_user, true);
      if(result.Succeeded) return true;
      return false;
    }    

    if(_user.TwoFactorEnabled)
    {
      var result = await _userManager.SetTwoFactorEnabledAsync(_user, false);
      if(result.Succeeded) return false;
      return true;
    }

    return _user.TwoFactorEnabled;
  }

  public async Task<bool> VerifyEmail(string email,string phoneNumber )
  {

    var _user = await _userManager.FindByNameAsync(phoneNumber);

    if(_user == null)
    {
      return false;
    }

    var isConfirmed =  await _userManager.IsEmailConfirmedAsync(_user);

    if(isConfirmed  || _user == null ){
      return  false;
    }

    var token = await _userManager.GenerateEmailConfirmationTokenAsync(_user);

    var confirmationLink = $"http://localhost:5233/api/Authentication/confirm-email?userId={_user.Id}&token={token}";

    
      await _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your email by <a href='{confirmationLink}'>clicking here</a>;.");

    return true;

  }

  public async Task<bool> SendTwoFactorCode(string phoneNumber)
  {

    var _user = await _userManager.FindByNameAsync(phoneNumber);

    if(_user == null ){
      return false;
    }

    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(_user, phoneNumber);
  

    try{
    await _smsSender.SendSmsAsync($"{phoneNumber}",$"your Two Factor Authentication code is {code}"); 
    return true;

    }catch(Exception ex){
      _logger.LogInformation($"{ex}");
      return false;
    }

  }

  public string GenerateRefreshToken(string phoneNumber)
  {
      var jwtKey = _configuration["jwtSettings:Key"] ?? "my-ultra-secure-and-ultra-long-secret";
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {         
          new Claim("phoneNumber", phoneNumber),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
      };

      var refreshToken = new JwtSecurityToken(
          _configuration["jwtSettings:Issuer"],
          _configuration["jwtSettings:Issuer"],
          claims,
          expires: DateTime.Now.AddDays(7),
          signingCredentials: credentials
      );

       return new JwtSecurityTokenHandler().WriteToken(refreshToken);
  }
  
  public async Task<AuthResponseDto> VerifyRefreshToken(string refreshToken)
  {
    _logger.LogInformation($"{refreshToken}");
    var jwtKey = _configuration["jwtSettings:Key"] ?? "my-ultra-secure-and-ultra-long-secret";
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = _configuration["jwtSettings:Issuer"],
        ValidAudience = _configuration["jwtSettings:Issuer"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero 
    };

    var tokenHandler = new JwtSecurityTokenHandler();       

    try
    {
      var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);
    
      if (validatedToken is not JwtSecurityToken jwtToken ||
          !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      {          
          return new AuthResponseDto{ErrorMessage="Invalid token"};
      }     

      var phoneNumber = principal.FindFirst("phoneNumber")?.Value;
      if (string.IsNullOrEmpty(phoneNumber))
      {
          return new AuthResponseDto{ErrorMessage="Invalid refresh token , no phone number"};
      }

      var newToken = GenerateJWTToken();
      var newRefreshToken = GenerateRefreshToken(phoneNumber);
      var userApi = await _userManager.Users
        .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);  
        var user = await _context.User.Where(u=>u.Id == userApi.Id).FirstOrDefaultAsync();

      return new AuthResponseDto{Token=newToken,RefreshToken=newRefreshToken,UserId=userApi.Id,ImageUrl=user.ImageUrl,IsTwoFactorEnabled=userApi.TwoFactorEnabled};
    }
    catch (Exception ex)
    {        
        return new AuthResponseDto{ErrorMessage="internal server error"};
    }
  }

}






