using Microsoft.EntityFrameworkCore.Migrations;

namespace Casheesh.Migrations
{
    public partial class RemoveUniqueOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Order",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Order",
                table: "Accounts",
                column: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Order",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Order",
                table: "Accounts",
                column: "Order",
                unique: true);
        }
    }
}
