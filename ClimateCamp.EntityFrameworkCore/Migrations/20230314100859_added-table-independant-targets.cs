using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedtableindependanttargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndependantTargets",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseLineYear = table.Column<int>(type: "int", nullable: false),
                    TargetYear = table.Column<int>(type: "int", nullable: true),
                    TSF = table.Column<int>(type: "int", nullable: true),
                    SBTI = table.Column<int>(type: "int", nullable: true),
                    Scope1Target = table.Column<float>(type: "real", nullable: true),
                    Scope2Target = table.Column<float>(type: "real", nullable: true),
                    Scope3Target = table.Column<float>(type: "real", nullable: true),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndependantTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndependantTargets_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndependantTargets_OrganizationUnitId",
                schema: "Master",
                table: "IndependantTargets",
                column: "OrganizationUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndependantTargets",
                schema: "Master");
        }
    }
}
