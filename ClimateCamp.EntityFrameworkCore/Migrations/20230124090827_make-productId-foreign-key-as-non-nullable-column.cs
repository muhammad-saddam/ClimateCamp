using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class makeproductIdforeignkeyasnonnullablecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_Products_ProductId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_Products_ProductId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_Products_ProductId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_Products_ProductId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
