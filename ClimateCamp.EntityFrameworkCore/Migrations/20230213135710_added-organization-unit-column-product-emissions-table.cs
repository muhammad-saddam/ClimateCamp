using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedorganizationunitcolumnproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_OrganizationUnits_OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_OrganizationUnits_OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                schema: "Transactions",
                table: "ProductEmissions");
        }
    }
}
