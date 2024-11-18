namespace Application.UserDto;

public class AuthResponseDto
{
    public  string? UserId { get; set; }
    public  string? Token { get; set; }
    public  string? RefreshToken { get; set; }

    public bool? IsTwoFactorEnabled { get; set; }

    public bool? IsEmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ErrorMessage {get;set;}

}



