using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class addedemissionfactorLibraryrelatioshiporganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_Units_UnitId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.AddColumn<Guid>(
                name: "EmissionsFactorsLibraryId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                schema: "Transactions",
                table: "ActivityData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EmissionsFactorsLibraryId",
                table: "Organizations",
                column: "EmissionsFactorsLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData",
                column: "ActivityTypeId",
                principalSchema: "Reference",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_Units_UnitId",
                schema: "Transactions",
                table: "ActivityData",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_EmissionsFactorsLibraries_EmissionsFactorsLibraryId",
                table: "Organizations",
                column: "EmissionsFactorsLibraryId",
                principalSchema: "Reference",
                principalTable: "EmissionsFactorsLibraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_Units_UnitId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_EmissionsFactorsLibraries_EmissionsFactorsLibraryId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_EmissionsFactorsLibraryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmissionsFactorsLibraryId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                schema: "Transactions",
                table: "ActivityData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "ActivityData",
                column: "ActivityTypeId",
                principalSchema: "Reference",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_Units_UnitId",
                schema: "Transactions",
                table: "ActivityData",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
