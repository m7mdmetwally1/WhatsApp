using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateSeenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "SeenBy",
                table: "Messages");

         

            migrationBuilder.CreateTable(
                name: "SeenBy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    MessagesId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    SeenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeenWith = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeenBy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeenBy_Messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

      

            migrationBuilder.CreateIndex(
                name: "IX_SeenBy_MessagesId",
                table: "SeenBy",
                column: "MessagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeenBy");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13334fc5-a37e-498f-9a78-5d04aca1d4c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fcf7cf3-352b-4035-a43e-c448a4c67664");

            migrationBuilder.DropColumn(
                name: "SeenTime",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "SeenBy",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "271f455a-5349-4309-ab48-9ef81deac21a", null, "Adminstrator", "ADMINSTRATOR" },
                    { "9a3aac27-f1d0-43de-a65d-3451e7952359", null, "User", "USER" }
                });
        }
    }
}
