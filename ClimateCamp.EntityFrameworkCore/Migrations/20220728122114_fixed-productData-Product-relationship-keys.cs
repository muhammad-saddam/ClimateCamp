using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixedproductDataProductrelationshipkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProductsData_ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProductsData_Products_ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProductsData_Products_ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProductsData_ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData");
        }
    }
}
