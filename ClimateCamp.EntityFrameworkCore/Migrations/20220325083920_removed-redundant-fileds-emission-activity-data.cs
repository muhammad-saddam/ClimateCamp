using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class removedredundantfiledsemissionactivitydata : Migration
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

            migrationBuilder.DropColumn(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: true);

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
