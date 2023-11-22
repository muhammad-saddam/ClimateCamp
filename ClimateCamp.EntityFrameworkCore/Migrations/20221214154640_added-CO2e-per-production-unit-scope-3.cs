using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedCO2eperproductionunitscope3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "tCO2ePPU",
                schema: "Master",
                table: "EmissionsSummaryScope3Details",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tCO2ePPU",
                schema: "Master",
                table: "EmissionsSummaryScope3Details");
        }
    }
}
