using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class offsetorganizationnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offsets_Organizations_OrganizationId",
                table: "Offsets");

            migrationBuilder.DropForeignKey(
                name: "FK_Offsets_Units_PriceUnitId",
                table: "Offsets");

            migrationBuilder.DropIndex(
                name: "IX_Offsets_PriceUnitId",
                table: "Offsets");

            migrationBuilder.DropColumn(
                name: "PriceUnitId",
                table: "Offsets");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Offsets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MaximumVolume",
                table: "Offsets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_Offsets_Organizations_OrganizationId",
                table: "Offsets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offsets_Organizations_OrganizationId",
                table: "Offsets");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Offsets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MaximumVolume",
                table: "Offsets",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceUnitId",
                table: "Offsets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offsets_PriceUnitId",
                table: "Offsets",
                column: "PriceUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offsets_Organizations_OrganizationId",
                table: "Offsets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offsets_Units_PriceUnitId",
                table: "Offsets",
                column: "PriceUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
