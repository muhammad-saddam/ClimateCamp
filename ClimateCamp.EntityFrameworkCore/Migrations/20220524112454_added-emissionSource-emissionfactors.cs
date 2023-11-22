using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedemissionSourceemissionfactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AddColumn<Guid>(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmissionSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId",
                principalSchema: "Reference",
                principalTable: "EmissionsFactors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropColumn(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropColumn(
                name: "EmissionSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AddColumn<int>(
                name: "EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId1",
                unique: true,
                filter: "[EmissionsSourceId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId1",
                principalSchema: "Reference",
                principalTable: "EmissionsSources",
                principalColumn: "Id");
        }
    }
}
