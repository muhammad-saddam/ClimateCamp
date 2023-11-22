using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedtablesciencebasedtargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScienceBasedTargets",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SBTI = table.Column<int>(type: "int", nullable: true),
                    NearTermTarget = table.Column<int>(type: "int", nullable: true),
                    NearTermTargetYear = table.Column<int>(type: "int", nullable: true),
                    LongTermTarget = table.Column<int>(type: "int", nullable: true),
                    LongTermTargetYear = table.Column<int>(type: "int", nullable: true),
                    NetZeroCommitted = table.Column<int>(type: "int", nullable: true),
                    NetZeroYear = table.Column<int>(type: "int", nullable: true),
                    OrganizationTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScienceBasedTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScienceBasedTargets_OrganizationTargets_OrganizationTargetId",
                        column: x => x.OrganizationTargetId,
                        principalSchema: "Master",
                        principalTable: "OrganizationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScienceBasedTargets_OrganizationTargetId",
                schema: "Master",
                table: "ScienceBasedTargets",
                column: "OrganizationTargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScienceBasedTargets",
                schema: "Master");
        }
    }
}
