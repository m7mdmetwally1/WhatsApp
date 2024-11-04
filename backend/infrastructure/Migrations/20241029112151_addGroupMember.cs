using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addGroupMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatUser",
                table: "GroupChatUser");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatUser_UserId",
                table: "GroupChatUser");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43195bfe-b449-4d21-bd4f-f015fdfa6e7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8aaa2eb5-0bd0-4dc9-8c3e-8eb664bf8b44");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenTime",
                table: "IndividualMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenTime",
                table: "GroupMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupCreatorId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatUser",
                table: "GroupChatUser",
                columns: new[] { "UserId", "GroupChatId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40fa6172-7d23-48c0-ab71-9dd9e7a8292e", null, "User", "USER" },
                    { "fed643d9-f3d0-4515-9331-dfb3e8e7ac48", null, "Adminstrator", "ADMINSTRATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatUser_GroupChatId",
                table: "GroupChatUser",
                column: "GroupChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatUser",
                table: "GroupChatUser");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatUser_GroupChatId",
                table: "GroupChatUser");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40fa6172-7d23-48c0-ab71-9dd9e7a8292e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed643d9-f3d0-4515-9331-dfb3e8e7ac48");

            migrationBuilder.DropColumn(
                name: "GroupCreatorId",
                table: "Chat");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenTime",
                table: "IndividualMessages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenTime",
                table: "GroupMessages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatUser",
                table: "GroupChatUser",
                columns: new[] { "GroupChatId", "UserId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43195bfe-b449-4d21-bd4f-f015fdfa6e7c", null, "Adminstrator", "ADMINSTRATOR" },
                    { "8aaa2eb5-0bd0-4dc9-8c3e-8eb664bf8b44", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatUser_UserId",
                table: "GroupChatUser",
                column: "UserId");
        }
    }
}
