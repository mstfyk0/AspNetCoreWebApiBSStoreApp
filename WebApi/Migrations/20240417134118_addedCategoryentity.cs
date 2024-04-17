using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class addedCategoryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fe6d935-8465-46d1-a608-c58912305824");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7a7afec-d79a-46b5-a8cb-a56be11ecab2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e41104b4-8b42-40ea-ba7d-415ac97c7647");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29806d50-074e-4f84-917b-09aff42bbea0", "9e266d0b-30c7-49ae-83cb-9a6eeee12905", "Admin", "ADMIN" },
                    { "b51b0218-a2f2-4bb6-88a8-641e755bf338", "987d501f-c81e-4fff-b871-6d7e257cc6be", "User", "USER" },
                    { "e3891cd9-d1b0-4b21-8743-d7693631382c", "3d988594-da55-4bf5-a4ab-7dedfd1090c2", "Editpor", "EDITOR" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Book" },
                    { 2, "Computer science" },
                    { 3, "Data science" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29806d50-074e-4f84-917b-09aff42bbea0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b51b0218-a2f2-4bb6-88a8-641e755bf338");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3891cd9-d1b0-4b21-8743-d7693631382c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7fe6d935-8465-46d1-a608-c58912305824", "275e9c24-ee6d-4321-8f64-d39f00b7bdb4", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a7a7afec-d79a-46b5-a8cb-a56be11ecab2", "5246c1ea-3721-4039-98d1-3f2ba257c17e", "Editpor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e41104b4-8b42-40ea-ba7d-415ac97c7647", "1911a479-6326-4978-8a9c-27ea0dbfc230", "Admin", "ADMIN" });
        }
    }
}
