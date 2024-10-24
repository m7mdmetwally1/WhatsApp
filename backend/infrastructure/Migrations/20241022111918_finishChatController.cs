using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class finishChatController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_AspNetUsers_ApiUserId",
                table: "Friend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c41ea29-e239-4227-81c1-eaaa8974b85a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dadfa98c-5274-4ea7-a74e-18aeec339c16");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "Friends");

            migrationBuilder.RenameColumn(
                name: "ApiUserId",
                table: "Friends",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_ApiUserId",
                table: "Friends",
                newName: "IX_Friends_UserId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "IndividualMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenTime",
                table: "GroupMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "GroupMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomName",
                table: "Friends",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7a6cdd01-18d5-4ca9-8a31-992fce279496", null, "User", "USER" },
                    { "f107f718-19ae-48e6-a4ca-bdaea926951c", null, "Adminstrator", "ADMINSTRATOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a6cdd01-18d5-4ca9-8a31-992fce279496");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f107f718-19ae-48e6-a4ca-bdaea926951c");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "IndividualMessages");

            migrationBuilder.DropColumn(
                name: "SeenTime",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "CustomName",
                table: "Friends");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "Friend");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friend",
                newName: "ApiUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_UserId",
                table: "Friend",
                newName: "IX_Friend_ApiUserId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friend",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8c41ea29-e239-4227-81c1-eaaa8974b85a", null, "User", "USER" },
                    { "dadfa98c-5274-4ea7-a74e-18aeec339c16", null, "Adminstrator", "ADMINSTRATOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_AspNetUsers_ApiUserId",
                table: "Friend",
                column: "ApiUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
