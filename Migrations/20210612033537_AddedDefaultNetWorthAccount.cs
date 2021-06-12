using Microsoft.EntityFrameworkCore.Migrations;

namespace Casheesh.Migrations
{
    public partial class AddedDefaultNetWorthAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Name", "CurrentBalance", "Order" },
                values: new object[] { "Net Worth", 0.0, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Name",
                keyValue: "Net Worth");
        }
    }
}
