using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsEmissionSources_ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "ProductEmissionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsEmissionSources_ProductEmissions_ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "ProductEmissionsId",
                principalSchema: "Transactions",
                principalTable: "ProductEmissions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsEmissionSources_ProductEmissions_ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropIndex(
                name: "IX_ProductsEmissionSources_ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "ProductEmissionsId",
                schema: "Reference",
                table: "ProductsEmissionSources");
        }
    }
}
