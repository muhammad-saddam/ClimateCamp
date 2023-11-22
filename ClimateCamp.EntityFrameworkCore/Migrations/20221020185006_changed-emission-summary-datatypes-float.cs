using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class changedemissionsummarydatatypesfloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Scope3tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Scope3PrimaryDataShare",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Scope2tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Scope1tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "ProductionQuantity",
                schema: "Master",
                table: "EmissionsSummary",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Scope3tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Scope3PrimaryDataShare",
                schema: "Master",
                table: "EmissionsSummary",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Scope2tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Scope1tCO2e",
                schema: "Master",
                table: "EmissionsSummary",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ProductionQuantity",
                schema: "Master",
                table: "EmissionsSummary",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
