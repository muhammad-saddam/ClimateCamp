using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedtotalco2ecolumnemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCO2E",
                schema: "Transactions",
                table: "Emissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalCO2E",
                schema: "Transactions",
                table: "Emissions",
                type: "real",
                nullable: true);
        }
    }
}
