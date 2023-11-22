using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class BaseLineTargetRelatedChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseLineEmission",
                table: "Organizations",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BaseLineYear",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "Organizations",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnits_CountryId",
                table: "OrganizationUnits",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUnits_Countries_CountryId",
                table: "OrganizationUnits",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUnits_Countries_CountryId",
                table: "OrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnits_CountryId",
                table: "OrganizationUnits");

            migrationBuilder.DropColumn(
                name: "BaseLineEmission",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "BaseLineYear",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Organizations");
        }
    }
}
