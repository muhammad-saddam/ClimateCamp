using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedcompositekeyfromProductsEmissionSourcesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsEmissionSources",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsEmissionSources",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsEmissionSources_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsEmissionSources",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropIndex(
                name: "IX_ProductsEmissionSources_ProductId",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsEmissionSources",
                schema: "Reference",
                table: "ProductsEmissionSources",
                columns: new[] { "ProductId", "EmissionsSourceId" });
        }
    }
}
