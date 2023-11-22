using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class Addedauditfieldstoemissionssummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Audited",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Auditor",
                schema: "Master",
                table: "EmissionsSummary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificate",
                schema: "Master",
                table: "EmissionsSummary",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audited",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "Auditor",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "Certificate",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
