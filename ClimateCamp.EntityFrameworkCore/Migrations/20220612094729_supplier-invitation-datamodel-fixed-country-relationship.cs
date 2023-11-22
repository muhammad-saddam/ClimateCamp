using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class supplierinvitationdatamodelfixedcountryrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CustomerSuppliers");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CustomerSuppliers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSuppliers_CountryId",
                table: "CustomerSuppliers",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSuppliers_Countries_CountryId",
                table: "CustomerSuppliers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSuppliers_Countries_CountryId",
                table: "CustomerSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSuppliers_CountryId",
                table: "CustomerSuppliers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CustomerSuppliers");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CustomerSuppliers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
