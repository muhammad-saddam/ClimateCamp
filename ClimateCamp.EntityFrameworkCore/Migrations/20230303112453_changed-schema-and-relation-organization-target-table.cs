using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class changedschemaandrelationorganizationtargettable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OrganizationTargets",
                schema: "Reference",
                newName: "OrganizationTargets",
                newSchema: "Master");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTargets_OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationTargets_OrganizationUnits_OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationTargets_OrganizationUnits_OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationTargets_OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.RenameTable(
                name: "OrganizationTargets",
                schema: "Master",
                newName: "OrganizationTargets",
                newSchema: "Reference");
        }
    }
}
