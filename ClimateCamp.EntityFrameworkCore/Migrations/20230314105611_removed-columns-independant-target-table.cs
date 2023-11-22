using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class removedcolumnsindependanttargettable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SBTI",
                schema: "Master",
                table: "IndependantTargets");

            migrationBuilder.DropColumn(
                name: "TSF",
                schema: "Master",
                table: "IndependantTargets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SBTI",
                schema: "Master",
                table: "IndependantTargets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TSF",
                schema: "Master",
                table: "IndependantTargets",
                type: "int",
                nullable: true);
        }
    }
}
