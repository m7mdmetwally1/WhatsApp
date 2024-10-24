using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editIndividualChatUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15745cea-872e-44e2-b72e-1ca81b246832");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9f6e794-5fd1-4371-8d1c-1bfda5857efc");

            migrationBuilder.AlterColumn<string>(
                name: "CustomName",
                table: "IndividualChatUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9fb5e098-2c09-44bf-b4af-d395d91edd44", null, "User", "USER" },
                    { "b67251ee-5c8a-4189-9bea-cd54427bb775", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fb5e098-2c09-44bf-b4af-d395d91edd44");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b67251ee-5c8a-4189-9bea-cd54427bb775");

            migrationBuilder.AlterColumn<string>(
                name: "CustomName",
                table: "IndividualChatUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15745cea-872e-44e2-b72e-1ca81b246832", null, "User", "USER" },
                    { "b9f6e794-5fd1-4371-8d1c-1bfda5857efc", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
