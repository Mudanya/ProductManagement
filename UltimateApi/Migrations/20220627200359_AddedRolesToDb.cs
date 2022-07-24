using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateApi.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "48a03f8b-395d-47fe-a9b5-517ccff211d5", "3c855e9c-5b4d-4074-b284-5100b265ce6e", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c8138c31-49da-4b35-95bf-f2161cbffa8e", "06ff9ec0-9caf-437b-ac55-d20d2f48ec70", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48a03f8b-395d-47fe-a9b5-517ccff211d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8138c31-49da-4b35-95bf-f2161cbffa8e");
        }
    }
}
