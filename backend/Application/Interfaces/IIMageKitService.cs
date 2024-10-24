using Microsoft.AspNetCore.Mvc;
using Application.UserDto;

namespace Application.Interfaces;

public interface IImageKitService
{
      Task<ImageKitResponse>  UploadImage(IFormFile imageUrl);
      Task<bool>  DeleteImage(string imageUrl);
}


