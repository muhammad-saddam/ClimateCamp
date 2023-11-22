using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class conversionFactorschangestoaccomodateallcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionFactors_Products_ProductId",
                schema: "Master",
                table: "ConversionFactors");

            migrationBuilder.RenameColumn(
                name: "ProductUnit",
                schema: "Master",
                table: "ConversionFactors",
                newName: "ConversionUnit");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Master",
                table: "ConversionFactors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityDataId",
                schema: "Master",
                table: "ConversionFactors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversionFactors_ActivityDataId",
                schema: "Master",
                table: "ConversionFactors",
                column: "ActivityDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionFactors_ActivityData_ActivityDataId",
                schema: "Master",
                table: "ConversionFactors",
                column: "ActivityDataId",
                principalSchema: "Transactions",
                principalTable: "ActivityData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionFactors_Products_ProductId",
                schema: "Master",
                table: "ConversionFactors",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversionFactors_ActivityData_ActivityDataId",
                schema: "Master",
                table: "ConversionFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversionFactors_Products_ProductId",
                schema: "Master",
                table: "ConversionFactors");

            migrationBuilder.DropIndex(
                name: "IX_ConversionFactors_ActivityDataId",
                schema: "Master",
                table: "ConversionFactors");

            migrationBuilder.DropColumn(
                name: "ActivityDataId",
                schema: "Master",
                table: "ConversionFactors");

            migrationBuilder.RenameColumn(
                name: "ConversionUnit",
                schema: "Master",
                table: "ConversionFactors",
                newName: "ProductUnit");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Master",
                table: "ConversionFactors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionFactors_Products_ProductId",
                schema: "Master",
                table: "ConversionFactors",
                column: "ProductId",
                principalSchema: "Reference",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
