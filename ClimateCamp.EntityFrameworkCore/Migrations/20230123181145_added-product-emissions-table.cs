using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedproductemissionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductEmissions",
                schema: "Reference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CO2eq = table.Column<float>(type: "real", nullable: true),
                    CO2eqUnitId = table.Column<int>(type: "int", nullable: true),
                    PrimaryDataShare = table.Column<float>(type: "real", nullable: true),
                    InventoryType = table.Column<int>(type: "int", nullable: true),
                    Audited = table.Column<bool>(type: "bit", nullable: true),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auditor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    PeriodType = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEmissions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Reference",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_ProductId",
                schema: "Reference",
                table: "ProductEmissions",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEmissions",
                schema: "Reference");
        }
    }
}
