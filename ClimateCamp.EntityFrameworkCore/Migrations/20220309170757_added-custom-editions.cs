using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedcustomeditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpEditions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsContactSales",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceLabel",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpEditions");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AbpEditions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpEditions");

            migrationBuilder.DropColumn(
                name: "IsContactSales",
                table: "AbpEditions");

            migrationBuilder.DropColumn(
                name: "PriceLabel",
                table: "AbpEditions");
        }
    }
}
