using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixemissionfactorfkissue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                unique: true,
                filter: "[EmissionsSourceId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AddColumn<Guid>(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId",
                principalSchema: "Reference",
                principalTable: "EmissionsFactors",
                principalColumn: "Id");
        }
    }
}
