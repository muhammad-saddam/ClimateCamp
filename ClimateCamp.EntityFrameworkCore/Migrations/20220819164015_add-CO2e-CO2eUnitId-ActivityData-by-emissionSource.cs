using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addCO2eCO2eUnitIdActivityDatabyemissionSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CO2e",
                schema: "Transactions",
                table: "StationaryCombustionData",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "StationaryCombustionData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CO2e",
                schema: "Transactions",
                table: "PurchasedEnergyData",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "PurchasedEnergyData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CO2e",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CO2e",
                schema: "Transactions",
                table: "StationaryCombustionData");

            migrationBuilder.DropColumn(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "StationaryCombustionData");

            migrationBuilder.DropColumn(
                name: "CO2e",
                schema: "Transactions",
                table: "PurchasedEnergyData");

            migrationBuilder.DropColumn(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "PurchasedEnergyData");

            migrationBuilder.DropColumn(
                name: "CO2e",
                schema: "Transactions",
                table: "MobileCombustionData");

            migrationBuilder.DropColumn(
                name: "CO2eUnitId",
                schema: "Transactions",
                table: "MobileCombustionData");
        }
    }
}
