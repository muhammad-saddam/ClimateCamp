using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class emissiontoactivitymappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emissions_ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_ActivityData_ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityDataId",
                principalSchema: "Transactions",
                principalTable: "ActivityData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_ActivityData_ActivityDataId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropIndex(
                name: "IX_Emissions_ActivityDataId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropColumn(
                name: "ActivityDataId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
