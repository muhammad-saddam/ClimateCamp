using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class emissiontblfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CO2eFactor",
                schema: "Transactions",
                table: "Emissions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eFactorUnitId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CO2eFactor",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropColumn(
                name: "CO2eFactorUnitId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Transactions",
                table: "Emissions");
        }
    }
}
