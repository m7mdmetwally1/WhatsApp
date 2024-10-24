using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editFriend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d324275-0b8a-4146-a260-a5cb165d91a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "faffd6aa-ae0f-45ad-8b93-4ba1d123dc3e");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15745cea-872e-44e2-b72e-1ca81b246832", null, "User", "USER" },
                    { "b9f6e794-5fd1-4371-8d1c-1bfda5857efc", null, "Adminstrator", "ADMINSTRATOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15745cea-872e-44e2-b72e-1ca81b246832");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9f6e794-5fd1-4371-8d1c-1bfda5857efc");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d324275-0b8a-4146-a260-a5cb165d91a2", null, "Adminstrator", "ADMINSTRATOR" },
                    { "faffd6aa-ae0f-45ad-8b93-4ba1d123dc3e", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_User_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
