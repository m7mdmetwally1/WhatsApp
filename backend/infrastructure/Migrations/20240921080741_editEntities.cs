using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

      migrationBuilder.DropForeignKey(name: "FK_ChatUser_Chat_ChatId", table: "ChatUser");
      migrationBuilder.DropForeignKey(name: "FK_Messages_User_UserId", table: "Messages");
      migrationBuilder.DropForeignKey(name: "FK_Messages_Chat_ChatId", table: "Messages");
      migrationBuilder.DropForeignKey(name: "FK_ChatUser_User_UserId", table: "ChatUser");





        migrationBuilder.DropTable("Chat");
        migrationBuilder.DropTable("User");
        migrationBuilder.DropTable("ChatUser");
        migrationBuilder.DropTable("Messages");


  
    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
