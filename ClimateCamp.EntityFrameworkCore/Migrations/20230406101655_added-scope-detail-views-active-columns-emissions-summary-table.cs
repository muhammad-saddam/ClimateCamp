using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedscopedetailviewsactivecolumnsemissionssummarytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScope1DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsScope2DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScope1DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary");

            migrationBuilder.DropColumn(
                name: "IsScope2DetailViewActive",
                schema: "Master",
                table: "EmissionsSummary");
        }
    }
}
