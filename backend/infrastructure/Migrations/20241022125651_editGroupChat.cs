using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editGroupChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fb5e098-2c09-44bf-b4af-d395d91edd44");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b67251ee-5c8a-4189-9bea-cd54427bb775");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35b3c616-d63d-4461-a093-c5180a513842", null, "User", "USER" },
                    { "e6441c73-29fe-4075-925b-4dfac1e3740e", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35b3c616-d63d-4461-a093-c5180a513842");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6441c73-29fe-4075-925b-4dfac1e3740e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9fb5e098-2c09-44bf-b4af-d395d91edd44", null, "User", "USER" },
                    { "b67251ee-5c8a-4189-9bea-cd54427bb775", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
