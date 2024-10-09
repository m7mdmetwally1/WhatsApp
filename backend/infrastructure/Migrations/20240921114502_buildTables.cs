using Domain.Entities.chatEntities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class buildTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

                  migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                });

                        migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

                migrationBuilder.CreateTable(
    name: "ChatUser",
    columns: table => new
    {
        ChatId = table.Column<string>(type: "nvarchar(255)", nullable: false),  // Foreign key to Chat
        UserId = table.Column<string>(type: "nvarchar(255)", nullable: false)   // Foreign key to User
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_ChatUser", x => new { x.ChatId, x.UserId });  // Composite primary key
        table.ForeignKey(
            name: "FK_ChatUser_Chat_ChatId",
            column: x => x.ChatId,
            principalTable: "Chat",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_ChatUser_User_UserId",
            column: x => x.UserId,
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
               
               
               migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                     ChatId = table.Column<string>(type: "nvarchar(255)",nullable: false),
                      UserId = table.Column<string>(type: "nvarchar(255)",nullable: false),

                },
                constraints: table =>
             {
        table.PrimaryKey("PK_Messages", x => x.Id);
        table.ForeignKey(
            name: "FK_Messages_Chat_ChatId",
            column: x => x.ChatId,
            principalTable: "Chat",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_Messages_User_UserId",
            column: x => x.UserId,
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c1c562a-2e45-4336-ac39-56064e4e1ef4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc150a51-05ea-4144-b267-22fdb949d32c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7449fae0-7df4-48e8-b30b-ca81b64f1c35", null, "Adminstrator", "ADMINSTRATOR" },
                    { "a5a3a661-8eb7-4a28-a6d5-f27319f974ac", null, "User", "USER" }
                });
        }
    }
}
