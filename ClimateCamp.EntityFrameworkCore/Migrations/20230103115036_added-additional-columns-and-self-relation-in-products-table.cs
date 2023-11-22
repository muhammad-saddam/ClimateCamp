using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedadditionalcolumnsandselfrelationinproductstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Reference",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentProductId",
                schema: "Reference",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ProductionQuantity",
                schema: "Reference",
                table: "Products",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentProductId",
                schema: "Reference",
                table: "Products",
                column: "ParentProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_ParentProductId",
                schema: "Reference",
                table: "Products",
                column: "ParentProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_ParentProductId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ParentProductId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ParentProductId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductionQuantity",
                schema: "Reference",
                table: "Products");
        }
    }
}
