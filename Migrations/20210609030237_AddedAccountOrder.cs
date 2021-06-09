using Microsoft.EntityFrameworkCore.Migrations;

namespace Casheesh.Migrations
{
    public partial class AddedAccountOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Accounts SET [Order] = rowid;");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Order",
                table: "Accounts",
                column: "Order",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Order",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Accounts");
        }
    }
}
