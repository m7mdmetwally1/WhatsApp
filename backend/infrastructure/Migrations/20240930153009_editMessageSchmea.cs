using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editMessageSchmea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.AddColumn<string>(
                name: "SeenBy",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c60d331-7f74-4b91-b73f-f59b08e451d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95c35173-b188-43e7-b570-4ac3b2bbc829");

            migrationBuilder.DropColumn(
                name: "SeenBy",
                table: "Messages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8c88bb97-8568-4e4a-be86-3eae4b6654cf", null, "Adminstrator", "ADMINSTRATOR" },
                    { "c9e7aec4-392c-4947-a7b2-4098807e7c56", null, "User", "USER" }
                });
        }
    }
}
