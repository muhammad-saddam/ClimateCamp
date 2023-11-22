using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedredundanttransportModecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportMode",
                schema: "Transactions",
                table: "TransportAndDistributionData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransportMode",
                schema: "Transactions",
                table: "TransportAndDistributionData",
                type: "int",
                nullable: true);
        }
    }
}
