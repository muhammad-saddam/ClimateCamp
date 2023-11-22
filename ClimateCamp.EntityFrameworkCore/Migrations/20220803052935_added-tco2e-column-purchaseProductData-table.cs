using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedtco2ecolumnpurchaseProductDatatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "tCO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tCO2eq",
                schema: "Transactions",
                table: "PurchasedProductsData");
        }
    }
}
