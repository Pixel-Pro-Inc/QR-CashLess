using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class ex4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CalledForBill",
                table: "OrderItems",
                newName: "Confirmed");

            migrationBuilder.AddColumn<bool>(
                name: "Developer",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Developer",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Confirmed",
                table: "OrderItems",
                newName: "CalledForBill");
        }
    }
}
