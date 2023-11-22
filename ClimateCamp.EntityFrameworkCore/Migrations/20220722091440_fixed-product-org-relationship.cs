using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class fixedproductorgrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrganizationUnits_OrganizationUnitId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                newName: "IX_Products_OrganizationId");

            migrationBuilder.AddColumn<string>(
                name: "TargetNotifiers",
                table: "AbpUserNotifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetNotifiers",
                table: "AbpNotifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Organizations_OrganizationId",
                schema: "Reference",
                table: "Products",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Organizations_OrganizationId",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TargetNotifiers",
                table: "AbpUserNotifications");

            migrationBuilder.DropColumn(
                name: "TargetNotifiers",
                table: "AbpNotifications");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "Reference",
                table: "Products",
                newName: "OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_OrganizationId",
                schema: "Reference",
                table: "Products",
                newName: "IX_Products_OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrganizationUnits_OrganizationUnitId",
                schema: "Reference",
                table: "Products",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id");
        }
    }
}
