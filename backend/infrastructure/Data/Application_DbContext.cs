using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using infrastructure.Data.Configuration;
using Domain.Entities.chatEntities;

namespace infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApiUser>
{

   public DbSet<Chat> Chat { get; set; }
 
   public DbSet<ChatUser> ChatUser { get; set; }
   public DbSet<Messages> Messages { get; set; }
 
   public DbSet<User> User { get; set; }

   public DbSet<SeenBy> SeenBy { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
/*

                 migrationBuilder.CreateTable(
            name: "SeenBy",
            columns: table => new
            {
                Id = table.Column<string>(nullable: false),
                MessagesId = table.Column<string>(nullable: false),
                SeenTime = table.Column<DateTime>(nullable: false),
                SeenWith = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SeenBy", x => x.Id);
                table.ForeignKey(
                    name: "FK_SeenBy_Messages_MessagesId",
                    column: x => x.MessagesId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                     onDelete: ReferentialAction.Cascade
                    );
            });
*/
     

      modelBuilder.Entity<ChatUser>()
      .HasKey(cs => new { cs.ChatId, cs.UserId });

       modelBuilder.Entity<SeenBy>()
        .HasOne(sb => sb.Messages)  
        .WithMany(m => m.SeenBy)    
        .HasForeignKey(sb => sb.MessagesId); 

      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new RoleConfig());

    }
}

