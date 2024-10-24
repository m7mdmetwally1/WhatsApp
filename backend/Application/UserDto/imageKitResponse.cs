
using System.ComponentModel.DataAnnotations;

namespace Application.UserDto;

public class ImageKitResponse 
{       
    public required string ImageUrl { get; set; }
    public required string FileId { get; set; }
}
