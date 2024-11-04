using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using infrastructure.Data.Configuration;
using Domain.Entities.chatEntities;

namespace infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApiUser>
{

   public DbSet<GroupChat> Chat { get; set; }
   public DbSet<IndividualChat> IndividualChat { get; set; }
   public DbSet<GroupChatUser> GroupChatUser { get; set; }
   public DbSet<IndividualChatUser> IndividualChatUser { get; set; }
   public DbSet<GroupMessage> GroupMessages { get; set; }
   public DbSet<IndividualMessage> IndividualMessages { get; set; }
   public DbSet<User> User { get; set; }
   public DbSet<SeenBy> SeenBy { get; set; }
   public DbSet<Friend> Friends {get;set;}

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Friend>()
          .HasKey(f => new { f.UserId, f.Id });     

      modelBuilder.Entity<GroupChatUser>()
          .HasKey(gc => new { gc.UserId, gc.GroupChatId });    

      modelBuilder.Entity<SeenBy>()
      .HasKey(f => new { f.Id, f.MessagesId });

      modelBuilder.Entity<User>()
        .HasMany(e => e.GroupChats)
        .WithMany(e => e.Users)
        .UsingEntity<GroupChatUser>();

      modelBuilder.Entity<User>()
      .HasMany(e => e.IndividualChats)
      .WithMany(e => e.Users)
      .UsingEntity<IndividualChatUser>();

      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new RoleConfig());

    }
}

