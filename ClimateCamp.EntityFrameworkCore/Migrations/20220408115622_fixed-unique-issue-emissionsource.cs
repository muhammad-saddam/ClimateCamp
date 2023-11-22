using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class fixeduniqueissueemissionsource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_ActivityData_ActivityDataId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_ActivityData_ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityDataId",
                principalSchema: "Transactions",
                principalTable: "ActivityData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_ActivityData_ActivityDataId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityDataId",
                schema: "Transactions",
                table: "Emissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
    }
}
