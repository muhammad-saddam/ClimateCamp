using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class emissionfatorsremovedindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AddColumn<int>(
                name: "EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId1",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
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

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                unique: true,
                filter: "[EmissionsSourceId] IS NOT NULL");
        }
    }
}
