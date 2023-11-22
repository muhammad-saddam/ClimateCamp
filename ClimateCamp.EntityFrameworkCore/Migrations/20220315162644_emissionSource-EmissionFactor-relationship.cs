using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class emissionSourceEmissionFactorrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId");
        }
    }
}
