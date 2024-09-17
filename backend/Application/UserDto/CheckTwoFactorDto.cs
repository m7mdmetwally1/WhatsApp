using System.ComponentModel.DataAnnotations;

namespace Application.UserDto;

public class CheckTwoFactorDto
{
     
     [Required]
    public string PhoneNumber { get; set; }

}

