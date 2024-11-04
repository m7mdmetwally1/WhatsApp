using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editSeenBn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7cdd61e-4126-46c7-bdf0-43ed8ae0edea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6d5cf2e-4594-4dcf-aa92-14f00f807511");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "IndividualMessages");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "GroupMessages");

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

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "412c7582-14a0-4b1a-9b69-da3ef947248a", null, "Adminstrator", "ADMINSTRATOR" },
                    { "bc1235c6-240d-4937-a40f-cefb062e0f00", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "412c7582-14a0-4b1a-9b69-da3ef947248a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc1235c6-240d-4937-a40f-cefb062e0f00");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Chat");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenTime",
                table: "IndividualMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "IndividualMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "SenderName",
                table: "GroupMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c7cdd61e-4126-46c7-bdf0-43ed8ae0edea", null, "Adminstrator", "ADMINSTRATOR" },
                    { "f6d5cf2e-4594-4dcf-aa92-14f00f807511", null, "User", "USER" }
                });
        }
    }
}
