using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class replacetCO2eqbyCO2eqaddCO2eqUnitId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tCO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData",
                newName: "CO2eq");

            migrationBuilder.RenameColumn(
                name: "tCO2eq",
                schema: "Reference",
                table: "Products",
                newName: "CO2eq");

            migrationBuilder.AddColumn<int>(
                name: "CO2eqUnitId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eqUnitId",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CO2eqUnitId",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "CO2eqUnitId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData",
                newName: "tCO2eq");

            migrationBuilder.RenameColumn(
                name: "CO2eq",
                schema: "Reference",
                table: "Products",
                newName: "tCO2eq");
        }
    }
}
