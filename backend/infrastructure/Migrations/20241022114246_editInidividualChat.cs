using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editInidividualChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a6cdd01-18d5-4ca9-8a31-992fce279496");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f107f718-19ae-48e6-a4ca-bdaea926951c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d324275-0b8a-4146-a260-a5cb165d91a2", null, "Adminstrator", "ADMINSTRATOR" },
                    { "faffd6aa-ae0f-45ad-8b93-4ba1d123dc3e", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d324275-0b8a-4146-a260-a5cb165d91a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "faffd6aa-ae0f-45ad-8b93-4ba1d123dc3e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7a6cdd01-18d5-4ca9-8a31-992fce279496", null, "User", "USER" },
                    { "f107f718-19ae-48e6-a4ca-bdaea926951c", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
