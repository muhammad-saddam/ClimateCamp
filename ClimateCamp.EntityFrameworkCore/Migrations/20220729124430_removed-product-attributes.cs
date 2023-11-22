using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedproductattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Volume",
                schema: "Reference",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "Reference",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "Reference",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "Volume",
                schema: "Reference",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
