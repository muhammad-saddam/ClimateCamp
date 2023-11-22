using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewcolumncustomerorganizationinproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CustomerOrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_Organizations_CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CustomerOrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_Organizations_CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "CustomerOrganizationId",
                schema: "Transactions",
                table: "ProductEmissions");
        }
    }
}
