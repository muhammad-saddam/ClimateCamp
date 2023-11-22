using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class _792purchasegoodsandservicesinitialdatamodeling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_Units_CO2EUnitId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AlterColumn<int>(
                name: "CO2EUnitId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_Units_CO2EUnitId",
                schema: "Transactions",
                table: "Emissions",
                column: "CO2EUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_Units_CO2EUnitId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AlterColumn<int>(
                name: "CO2EUnitId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_Units_CO2EUnitId",
                schema: "Transactions",
                table: "Emissions",
                column: "CO2EUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
