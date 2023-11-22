using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixedemissionsourceissuefactormodelcreatingissue : Migration
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

            migrationBuilder.AddColumn<Guid>(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "EmissionsSourceId",
                principalSchema: "Reference",
                principalTable: "EmissionsSources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "EmissionsFactorsId",
                principalSchema: "Reference",
                principalTable: "EmissionsFactors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_EmissionsSources_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsSources_EmissionsFactors_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsSources_EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_EmissionsSourceId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "EmissionsFactorsId",
                schema: "Reference",
                table: "EmissionsSources");

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
    }
}
