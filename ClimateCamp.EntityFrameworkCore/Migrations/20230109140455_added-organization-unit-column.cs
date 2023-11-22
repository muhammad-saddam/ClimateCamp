using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedorganizationunitcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrganizationUnits_OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrganizationUnits_OrganizationUnitId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrganizationUnitId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                schema: "Reference",
                table: "Products");
        }
    }
}
