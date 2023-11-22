using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewcolumnsemissionssummarytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActiveScope1Emissions",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveScope2Emissions",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveScope3Emissions",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveScope1Emissions",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "IsActiveScope2Emissions",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "IsActiveScope3Emissions",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
