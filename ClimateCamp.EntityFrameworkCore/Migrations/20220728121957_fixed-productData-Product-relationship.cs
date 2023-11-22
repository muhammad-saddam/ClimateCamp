using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixedproductDataProductrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProductsData_Products_ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProductsData_ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.RenameColumn(
                name: "ContactPersonName",
                schema: "Transactions",
                table: "CustomerSuppliers",
                newName: "ContactLastName");

            migrationBuilder.AddColumn<string>(
                name: "ContactFirstName",
                schema: "Transactions",
                table: "CustomerSuppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactFirstName",
                schema: "Transactions",
                table: "CustomerSuppliers");

            migrationBuilder.RenameColumn(
                name: "ContactLastName",
                schema: "Transactions",
                table: "CustomerSuppliers",
                newName: "ContactPersonName");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProductsData_ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProductsData_Products_ProductId1",
                schema: "Transactions",
                table: "PurchasedProductsData",
                column: "ProductId1",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
