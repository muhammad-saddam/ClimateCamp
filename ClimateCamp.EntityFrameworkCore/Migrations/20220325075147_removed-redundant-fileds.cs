using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class removedredundantfileds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropIndex(
                name: "IX_Emissions_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                newName: "EmissionsDataQualityScore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmissionsDataQualityScore",
                schema: "Transactions",
                table: "Emissions",
                newName: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Emissions_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityTypeId",
                principalSchema: "Reference",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
