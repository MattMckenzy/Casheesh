using Microsoft.EntityFrameworkCore.Migrations;

namespace Casheesh.Migrations
{
    public partial class AddRateSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRate",
                table: "Recurrences",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRate",
                table: "Recurrences");
        }
    }
}
