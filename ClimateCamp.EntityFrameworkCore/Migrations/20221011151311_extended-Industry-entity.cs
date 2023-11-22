using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class extendedIndustryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultUnitId",
                table: "Industries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPriority",
                table: "Industries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentIndustryId",
                table: "Industries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Industries_DefaultUnitId",
                table: "Industries",
                column: "DefaultUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_ParentIndustryId",
                table: "Industries",
                column: "ParentIndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Industries_Industries_ParentIndustryId",
                table: "Industries",
                column: "ParentIndustryId",
                principalTable: "Industries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Industries_Units_DefaultUnitId",
                table: "Industries",
                column: "DefaultUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Industries_Industries_ParentIndustryId",
                table: "Industries");

            migrationBuilder.DropForeignKey(
                name: "FK_Industries_Units_DefaultUnitId",
                table: "Industries");

            migrationBuilder.DropIndex(
                name: "IX_Industries_DefaultUnitId",
                table: "Industries");

            migrationBuilder.DropIndex(
                name: "IX_Industries_ParentIndustryId",
                table: "Industries");

            migrationBuilder.DropColumn(
                name: "DefaultUnitId",
                table: "Industries");

            migrationBuilder.DropColumn(
                name: "IsPriority",
                table: "Industries");

            migrationBuilder.DropColumn(
                name: "ParentIndustryId",
                table: "Industries");
        }
    }
}
