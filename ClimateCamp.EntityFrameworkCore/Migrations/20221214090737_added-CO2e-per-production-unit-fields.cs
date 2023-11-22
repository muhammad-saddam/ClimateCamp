using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedCO2eperproductionunitfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCO2ePerProductionUnitActive",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Scope1CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scope2CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scope3CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCO2ePerProductionUnitActive",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "Scope1CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "Scope2CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "Scope3CO2ePPU",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
