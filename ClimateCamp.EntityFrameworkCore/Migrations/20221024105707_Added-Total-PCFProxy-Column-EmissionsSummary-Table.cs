using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class AddedTotalPCFProxyColumnEmissionsSummaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalPCfProxy",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPCfProxy",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
