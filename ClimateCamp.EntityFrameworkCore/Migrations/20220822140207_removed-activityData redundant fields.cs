using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedactivityDataredundantfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileCombustionData_Units_DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData");

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

            migrationBuilder.DropColumn(
                name: "IndustrialProcessType",
                schema: "Transactions",
                table: "MobileCombustionData");

            migrationBuilder.AlterColumn<int>(
                name: "DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Distance",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileCombustionData_Units_DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                column: "DistanceUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileCombustionData_Units_DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData");

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

            migrationBuilder.AlterColumn<int>(
                name: "DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Distance",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "IndustrialProcessType",
                schema: "Transactions",
                table: "MobileCombustionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MobileCombustionData_Units_DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                column: "DistanceUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
