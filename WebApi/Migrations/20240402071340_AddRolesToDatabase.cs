using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class AddRolesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e7f4fcd-d07a-46e7-a693-f13f981c5803", "b7d935af-1d5e-4ca4-b067-f73446d370da", "Editpor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b6573ba6-da81-4989-8889-ab6dab5bca61", "0dec173a-e092-4f19-b91d-5cb28796bd23", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fa88aa5b-87d0-42c2-a815-ed54bce661ca", "1f5555ad-def4-47fa-9ea9-d4bb33fef66a", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e7f4fcd-d07a-46e7-a693-f13f981c5803");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6573ba6-da81-4989-8889-ab6dab5bca61");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa88aa5b-87d0-42c2-a815-ed54bce661ca");
        }
    }
}
