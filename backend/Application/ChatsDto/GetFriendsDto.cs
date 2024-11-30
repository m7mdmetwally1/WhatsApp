namespace Application.ChatsDto;

public class GetFriendsDto
{
    public required string FriendId { get; set; }
    public required string FriendCustomName { get; set; }
    public required string ImageUrl { get; set; }
}