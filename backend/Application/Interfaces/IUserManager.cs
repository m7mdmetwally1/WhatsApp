using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Application.UserDto;
using Application.ChatsDto;
using Application.Common;
using Domain.Entities.chatEntities;


namespace Application.Interfaces;

public interface IUserManager
{
    Task<Result<ImageKitResponse>> UploadImage( IFormFile imageUrl,string? userId,string? groupChatId);
    Task<Result<bool>> DeleteUserImageKitImage(string userId );
    Task<Result<bool>> DeleteGroupImageKitImage(string groupChatId );    
    Task<Result<bool>> AddFriend(CreateIndividualChatDto chatDto);
    Task<Result<GetFriendsDto>> MyFriends(string userId);
}   
