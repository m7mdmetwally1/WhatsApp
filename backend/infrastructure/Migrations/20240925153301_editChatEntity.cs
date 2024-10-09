using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editChatEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupChat",
                table: "Chat",
                type: "bit",
                nullable: false,
                defaultValue: false);

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "033ec4e0-e737-4abe-8910-cc7dc3fbc7d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f30c6e6c-0ffe-43ab-b3bd-7dbb1d4ab836");

            migrationBuilder.DropColumn(
                name: "IsGroupChat",
                table: "Chat");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31230f77-897c-44cb-a133-d64ed33d1aa3", null, "Adminstrator", "ADMINSTRATOR" },
                    { "ce036379-d9e0-453b-be42-54c6dd0ca836", null, "User", "USER" }
                });
        }
    }
}
