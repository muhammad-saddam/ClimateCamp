using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewtableProductsEmissionSources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsEmissionSources",
                schema: "Reference",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmissionsSourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsEmissionSources", x => new { x.ProductId, x.EmissionsSourceId });
                    table.ForeignKey(
                        name: "FK_ProductsEmissionSources_EmissionsSources_EmissionsSourceId",
                        column: x => x.EmissionsSourceId,
                        principalSchema: "Reference",
                        principalTable: "EmissionsSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsEmissionSources_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Reference",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsEmissionSources_EmissionsSourceId",
                schema: "Reference",
                table: "ProductsEmissionSources",
                column: "EmissionsSourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsEmissionSources",
                schema: "Reference");
        }
    }
}
