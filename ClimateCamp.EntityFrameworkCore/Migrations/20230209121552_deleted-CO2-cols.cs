using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class deletedCO2cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "CO2eqUnitId",
                schema: "Transactions",
                table: "PurchasedProductsData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eqUnitId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "int",
                nullable: true);
        }
    }
}
