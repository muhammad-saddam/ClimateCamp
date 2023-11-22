using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class updatedschemaforconversionfactorstabletomaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConversionFactors",
                schema: "Reference",
                newName: "ConversionFactors",
                newSchema: "Master");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConversionFactors",
                schema: "Master",
                newName: "ConversionFactors",
                newSchema: "Reference");
        }
    }
}
