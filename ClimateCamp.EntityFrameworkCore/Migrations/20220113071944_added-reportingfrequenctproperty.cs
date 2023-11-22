using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedreportingfrequenctproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "Organizations",
                newName: "Target");

            migrationBuilder.AddColumn<int>(
                name: "ReportingFrequencyId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingFrequencyId",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "Organizations",
                newName: "Percentage");
        }
    }
}
