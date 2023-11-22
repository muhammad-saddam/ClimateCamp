using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedunitemissionfactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_UnitId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_Units_UnitId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_Units_UnitId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_UnitId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsFactors");
        }
    }
}
