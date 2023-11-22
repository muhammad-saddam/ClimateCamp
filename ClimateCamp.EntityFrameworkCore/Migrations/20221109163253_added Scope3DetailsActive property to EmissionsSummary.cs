using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedScope3DetailsActivepropertytoEmissionsSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScope3DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScope3DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
