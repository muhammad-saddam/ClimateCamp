using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedEnergyMixremovedIsRenewable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRenewable",
                schema: "Transactions",
                table: "PurchasedEnergyData");

            migrationBuilder.AddColumn<string>(
                name: "EnergyMix",
                schema: "Transactions",
                table: "PurchasedEnergyData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyMix",
                schema: "Transactions",
                table: "PurchasedEnergyData");

            migrationBuilder.AddColumn<bool>(
                name: "IsRenewable",
                schema: "Transactions",
                table: "PurchasedEnergyData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
