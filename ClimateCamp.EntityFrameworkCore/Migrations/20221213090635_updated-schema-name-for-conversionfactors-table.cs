using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class updatedschemanameforconversionfactorstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConversionFactors",
                schema: "Transactions",
                newName: "ConversionFactors",
                newSchema: "Reference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConversionFactors",
                schema: "Reference",
                newName: "ConversionFactors",
                newSchema: "Transactions");
        }
    }
}
