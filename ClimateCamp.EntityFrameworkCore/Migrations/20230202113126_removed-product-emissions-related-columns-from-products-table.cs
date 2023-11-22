using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedproductemissionsrelatedcolumnsfromproductstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audited",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Auditor",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CO2eq",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CO2eqUnitId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Certificate",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InventoryType",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Period",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PeriodType",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PrimaryDataShare",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Year",
                schema: "Reference",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Audited",
                schema: "Reference",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Auditor",
                schema: "Reference",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CO2eq",
                schema: "Reference",
                table: "Products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CO2eqUnitId",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificate",
                schema: "Reference",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryType",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodType",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PrimaryDataShare",
                schema: "Reference",
                table: "Products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "Reference",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
