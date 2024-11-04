using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editSeenId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeenBy",
                table: "SeenBy");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "412c7582-14a0-4b1a-9b69-da3ef947248a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc1235c6-240d-4937-a40f-cefb062e0f00");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeenBy",
                table: "SeenBy",
                columns: new[] { "Id", "MessagesId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43195bfe-b449-4d21-bd4f-f015fdfa6e7c", null, "Adminstrator", "ADMINSTRATOR" },
                    { "8aaa2eb5-0bd0-4dc9-8c3e-8eb664bf8b44", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeenBy",
                table: "SeenBy");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43195bfe-b449-4d21-bd4f-f015fdfa6e7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8aaa2eb5-0bd0-4dc9-8c3e-8eb664bf8b44");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeenBy",
                table: "SeenBy",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "412c7582-14a0-4b1a-9b69-da3ef947248a", null, "Adminstrator", "ADMINSTRATOR" },
                    { "bc1235c6-240d-4937-a40f-cefb062e0f00", null, "User", "USER" }
                });
        }
    }
}
