using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedorganizationrelationproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
