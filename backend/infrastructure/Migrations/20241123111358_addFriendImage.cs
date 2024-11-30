using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFriendImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a3bd546-3999-4642-b765-a170aa4a8215");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ad7c2c9-7819-49b4-9f5e-a85f2c038622");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Friends",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39a05232-11c4-4763-bcaf-598453a2bfcd", null, "User", "USER" },
                    { "3edfc88a-57c0-411e-aa47-bb194247c8f1", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39a05232-11c4-4763-bcaf-598453a2bfcd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3edfc88a-57c0-411e-aa47-bb194247c8f1");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Friends");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a3bd546-3999-4642-b765-a170aa4a8215", null, "Adminstrator", "ADMINSTRATOR" },
                    { "1ad7c2c9-7819-49b4-9f5e-a85f2c038622", null, "User", "USER" }
                });
        }
    }
}
