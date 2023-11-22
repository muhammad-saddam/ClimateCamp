using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class AddedEmisisonFactorIdInActivityDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityData_EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData",
                column: "EmissionFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_EmissionsFactors_EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData",
                column: "EmissionFactorId",
                principalSchema: "Reference",
                principalTable: "EmissionsFactors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_EmissionsFactors_EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropIndex(
                name: "IX_ActivityData_EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "EmissionFactorId",
                schema: "Transactions",
                table: "ActivityData");
        }
    }
}
