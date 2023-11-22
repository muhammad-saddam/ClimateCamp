using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewidentifiercolumnproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmissionSourceType",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmissionSourceType",
                schema: "Transactions",
                table: "ProductEmissions");
        }
    }
}
