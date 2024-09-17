
using System.ComponentModel.DataAnnotations;

namespace Application.UserDto;

public class LoginDto 
{
    
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Password { get; set; }
   

    

}
