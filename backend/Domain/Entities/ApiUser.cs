using Microsoft.AspNetCore.Identity;
using Domain.Entities.chatEntities;

namespace Domain.Entities;

public class ApiUser : IdentityUser
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}

