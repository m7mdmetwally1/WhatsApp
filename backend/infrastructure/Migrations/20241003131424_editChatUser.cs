using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editChatUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "CustomName2",
                table: "ChatUser");

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfe89547-f7fc-4f39-a934-b9caf2238422");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef89d701-3cbc-436f-a634-5b5e44da0d73");

            migrationBuilder.AddColumn<string>(
                name: "CustomName2",
                table: "ChatUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c60d331-7f74-4b91-b73f-f59b08e451d2", null, "User", "USER" },
                    { "95c35173-b188-43e7-b570-4ac3b2bbc829", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
