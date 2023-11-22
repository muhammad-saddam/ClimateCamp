using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class changedrelationshipbetweenProductEmissionsandCarbonFootprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_CarbonFootprints_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.AddForeignKey(
                name: "FK_CarbonFootprints_ProductEmissions_Id",
                schema: "Transactions",
                table: "CarbonFootprints",
                column: "Id",
                principalSchema: "Transactions",
                principalTable: "ProductEmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarbonFootprints_ProductEmissions_Id",
                schema: "Transactions",
                table: "CarbonFootprints");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CarbonFootprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_CarbonFootprints_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CarbonFootprintId",
                principalSchema: "Transactions",
                principalTable: "CarbonFootprints",
                principalColumn: "Id");
        }
    }
}
