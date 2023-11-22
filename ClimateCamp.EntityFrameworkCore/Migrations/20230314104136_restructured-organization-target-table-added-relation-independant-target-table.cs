using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class restructuredorganizationtargettableaddedrelationindependanttargettable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SBTI",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "Scope1Target",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "Scope2Target",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "Scope3Target",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "TSF",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "TargetYear",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.RenameColumn(
                name: "BaseLineYear",
                schema: "Master",
                table: "OrganizationTargets",
                newName: "TSFType");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "Master",
                table: "OrganizationTargets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTargets_OrganizationId",
                schema: "Master",
                table: "OrganizationTargets",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IndependantTargets_OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets",
                column: "OrganizationTargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndependantTargets_OrganizationTargets_OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets",
                column: "OrganizationTargetId",
                principalSchema: "Master",
                principalTable: "OrganizationTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationTargets_Organizations_OrganizationId",
                schema: "Master",
                table: "OrganizationTargets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndependantTargets_OrganizationTargets_OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationTargets_Organizations_OrganizationId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationTargets_OrganizationId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropIndex(
                name: "IX_IndependantTargets_OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "Master",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "OrganizationTargetId",
                schema: "Master",
                table: "IndependantTargets");

            migrationBuilder.RenameColumn(
                name: "TSFType",
                schema: "Master",
                table: "OrganizationTargets",
                newName: "BaseLineYear");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationUnitId",
                schema: "Master",
                table: "OrganizationTargets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SBTI",
                schema: "Master",
                table: "OrganizationTargets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scope1Target",
                schema: "Master",
                table: "OrganizationTargets",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scope2Target",
                schema: "Master",
                table: "OrganizationTargets",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scope3Target",
                schema: "Master",
                table: "OrganizationTargets",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TSF",
                schema: "Master",
                table: "OrganizationTargets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetYear",
                schema: "Master",
                table: "OrganizationTargets",
                type: "int",
                nullable: true);

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
    }
}
