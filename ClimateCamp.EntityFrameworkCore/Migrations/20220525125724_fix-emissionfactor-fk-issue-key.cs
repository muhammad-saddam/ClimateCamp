using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixemissionfactorfkissuekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "EmissionSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AlterColumn<int>(
                name: "EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                principalSchema: "Reference",
                principalTable: "EmissionsSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AlterColumn<int>(
                name: "EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmissionSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                unique: true,
                filter: "[EmissionsSourceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                principalSchema: "Reference",
                principalTable: "EmissionsSources",
                principalColumn: "Id");
        }
    }
}
