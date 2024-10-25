using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be65bd7a-480c-4ce1-bf7f-5be2e26a4685");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c73078b0-377d-401d-bd4a-c41ff397289a");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c7cdd61e-4126-46c7-bdf0-43ed8ae0edea", null, "Adminstrator", "ADMINSTRATOR" },
                    { "f6d5cf2e-4594-4dcf-aa92-14f00f807511", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ImageId",
                table: "User");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "be65bd7a-480c-4ce1-bf7f-5be2e26a4685", null, "User", "USER" },
                    { "c73078b0-377d-401d-bd4a-c41ff397289a", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
