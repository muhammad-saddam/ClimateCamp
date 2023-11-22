using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class renamedtableIndependantTargetToTargetIndependant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndependantTargets",
                schema: "Master");

            migrationBuilder.CreateTable(
                name: "TargetIndependant",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseLineYear = table.Column<int>(type: "int", nullable: false),
                    TargetYear = table.Column<int>(type: "int", nullable: true),
                    Scope1Target = table.Column<float>(type: "real", nullable: true),
                    Scope2Target = table.Column<float>(type: "real", nullable: true),
                    Scope3Target = table.Column<float>(type: "real", nullable: true),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetIndependant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetIndependant_OrganizationTargets_OrganizationTargetId",
                        column: x => x.OrganizationTargetId,
                        principalSchema: "Master",
                        principalTable: "OrganizationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetIndependant_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetIndependant_OrganizationTargetId",
                schema: "Master",
                table: "TargetIndependant",
                column: "OrganizationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetIndependant_OrganizationUnitId",
                schema: "Master",
                table: "TargetIndependant",
                column: "OrganizationUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetIndependant",
                schema: "Master");

            migrationBuilder.CreateTable(
                name: "IndependantTargets",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseLineYear = table.Column<int>(type: "int", nullable: false),
                    Scope1Target = table.Column<float>(type: "real", nullable: true),
                    Scope2Target = table.Column<float>(type: "real", nullable: true),
                    Scope3Target = table.Column<float>(type: "real", nullable: true),
                    TargetYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndependantTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndependantTargets_OrganizationTargets_OrganizationTargetId",
                        column: x => x.OrganizationTargetId,
                        principalSchema: "Master",
                        principalTable: "OrganizationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndependantTargets_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndependantTargets_OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets",
                column: "OrganizationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_IndependantTargets_OrganizationUnitId",
                schema: "Master",
                table: "IndependantTargets",
                column: "OrganizationUnitId");
        }
    }
}
