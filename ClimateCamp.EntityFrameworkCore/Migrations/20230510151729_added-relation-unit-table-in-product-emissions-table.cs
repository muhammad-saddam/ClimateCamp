using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedrelationunittableinproductemissionstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_CO2eqUnitId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CO2eqUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_Units_CO2eqUnitId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CO2eqUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_Units_CO2eqUnitId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_CO2eqUnitId",
                schema: "Transactions",
                table: "ProductEmissions");
        }
    }
}
