using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class AddedTblCountry_ChangedCountryColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "OrganizationUnits");
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "OrganizationUnits",
                type: "int",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Organizations");
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoCharCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreeCharCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "OrganizationUnits",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
