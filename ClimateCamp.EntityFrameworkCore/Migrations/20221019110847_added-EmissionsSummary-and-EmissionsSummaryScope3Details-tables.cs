using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedEmissionsSummaryandEmissionsSummaryScope3Detailstables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Master");

            migrationBuilder.CreateTable(
                name: "EmissionsSummary",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    ProductionQuantity = table.Column<double>(type: "float", nullable: false),
                    ProductionMetricId = table.Column<int>(type: "int", nullable: false),
                    Scope1tCO2e = table.Column<double>(type: "float", nullable: false),
                    Scope2tCO2e = table.Column<double>(type: "float", nullable: false),
                    Scope2Methodology = table.Column<int>(type: "int", nullable: false),
                    Scope3tCO2e = table.Column<double>(type: "float", nullable: false),
                    Scope3PrimaryDataShare = table.Column<double>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionsSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmissionsSummary_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmissionsSummary_Units_ProductionMetricId",
                        column: x => x.ProductionMetricId,
                        principalSchema: "Reference",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmissionsSummaryScope3Details",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmissionSummaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmissionSourceId = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: false),
                    tCO2e = table.Column<double>(type: "float", nullable: false),
                    Methodology = table.Column<int>(type: "int", nullable: false),
                    PrimaryDataShare = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionsSummaryScope3Details", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSummary_OrganizationUnitId",
                schema: "Master",
                table: "EmissionsSummary",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSummary_ProductionMetricId",
                schema: "Master",
                table: "EmissionsSummary",
                column: "ProductionMetricId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmissionsSummary",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "EmissionsSummaryScope3Details",
                schema: "Master");
        }
    }
}
