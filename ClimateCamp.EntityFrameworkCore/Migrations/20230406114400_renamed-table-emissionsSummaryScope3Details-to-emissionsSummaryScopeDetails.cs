using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class renamedtableemissionsSummaryScope3DetailstoemissionsSummaryScopeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "EmissionsSummaryScopeDetails",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmissionSummaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmissionSourceId = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: true),
                    tCO2e = table.Column<float>(type: "real", nullable: true),
                    tCO2ePPU = table.Column<float>(type: "real", nullable: true),
                    Methodology = table.Column<int>(type: "int", nullable: true),
                    PrimaryDataShare = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionsSummaryScopeDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmissionsSummaryScopeDetails",
                schema: "Master");

            migrationBuilder.CreateTable(
                name: "EmissionsSummaryScope3Details",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Availability = table.Column<int>(type: "int", nullable: true),
                    EmissionSourceId = table.Column<int>(type: "int", nullable: false),
                    EmissionSummaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Methodology = table.Column<int>(type: "int", nullable: true),
                    PrimaryDataShare = table.Column<float>(type: "real", nullable: true),
                    tCO2e = table.Column<float>(type: "real", nullable: true),
                    tCO2ePPU = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionsSummaryScope3Details", x => x.Id);
                });
        }
    }
}
