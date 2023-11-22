using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedorganizationrelationproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_Organizations_OrganizationId",
                schema: "Reference",
                table: "ProductEmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_OrganizationId",
                schema: "Reference",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "Reference",
                table: "ProductEmissions");

            migrationBuilder.RenameTable(
                name: "ProductEmissions",
                schema: "Reference",
                newName: "ProductEmissions",
                newSchema: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ProductEmissions",
                schema: "Transactions",
                newName: "ProductEmissions",
                newSchema: "Reference");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "Reference",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_OrganizationId",
                schema: "Reference",
                table: "ProductEmissions",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_Organizations_OrganizationId",
                schema: "Reference",
                table: "ProductEmissions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
