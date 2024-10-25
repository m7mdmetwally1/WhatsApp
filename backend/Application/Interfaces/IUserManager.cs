using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Application.UserDto;
using Application.ChatsDto;


namespace Application.Interfaces;

public interface IUserManager
{
    Task<ImageKitResponse> UploadImage( IFormFile imageUrl);
    Task<bool> DeleteUserImageKitImage(string imageId,string userId );
    Task<bool> AddFriend(CreateIndividualChatDto chatDto);
}