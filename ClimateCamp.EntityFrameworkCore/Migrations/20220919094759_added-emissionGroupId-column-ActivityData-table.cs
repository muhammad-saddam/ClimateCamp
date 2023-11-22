using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedemissionGroupIdcolumnActivityDatatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityData_EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData",
                column: "EmissionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityData_EmissionGroups_EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData",
                column: "EmissionGroupId",
                principalSchema: "Reference",
                principalTable: "EmissionGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityData_EmissionGroups_EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropIndex(
                name: "IX_ActivityData_EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "EmissionGroupId",
                schema: "Transactions",
                table: "ActivityData");
        }
    }
}
