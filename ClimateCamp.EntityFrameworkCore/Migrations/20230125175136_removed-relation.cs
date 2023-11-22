using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsEmissionSources_Products_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropIndex(
                name: "IX_ProductsEmissionSources_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            //migrationBuilder.AddColumn<int>(
            //    name: "Status",
            //    schema: "Transactions",
            //    table: "ActivityData",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Status",
            //    schema: "Transactions",
            //    table: "ActivityData");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductsEmissionSources_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsEmissionSources_Products_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
