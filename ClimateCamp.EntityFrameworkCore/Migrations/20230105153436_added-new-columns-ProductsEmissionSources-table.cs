using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewcolumnsProductsEmissionSourcestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Availability",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Methodology",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PrimaryDataShare",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "tCO2e",
                schema: "Reference",
                table: "ProductsEmissionSources",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "Methodology",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "PrimaryDataShare",
                schema: "Reference",
                table: "ProductsEmissionSources");

            migrationBuilder.DropColumn(
                name: "tCO2e",
                schema: "Reference",
                table: "ProductsEmissionSources");
        }
    }
}
