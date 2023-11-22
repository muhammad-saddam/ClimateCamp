using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedunitemissionSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSources_UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsSources_Units_UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsSources_Units_UnitId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsSources_UnitId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsSources");
        }
    }
}
