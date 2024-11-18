using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeuserentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40fa6172-7d23-48c0-ab71-9dd9e7a8292e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed643d9-f3d0-4515-9331-dfb3e8e7ac48");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a3bd546-3999-4642-b765-a170aa4a8215", null, "Adminstrator", "ADMINSTRATOR" },
                    { "1ad7c2c9-7819-49b4-9f5e-a85f2c038622", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a3bd546-3999-4642-b765-a170aa4a8215");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ad7c2c9-7819-49b4-9f5e-a85f2c038622");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "User");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40fa6172-7d23-48c0-ab71-9dd9e7a8292e", null, "User", "USER" },
                    { "fed643d9-f3d0-4515-9331-dfb3e8e7ac48", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
