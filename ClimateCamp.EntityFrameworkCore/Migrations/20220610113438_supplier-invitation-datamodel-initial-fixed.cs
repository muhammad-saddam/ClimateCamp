using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class supplierinvitationdatamodelinitialfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSuppliers_Organizations_SupplierOrganizationId",
                table: "CustomerSuppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSuppliers_Organizations_SupplierOrganizationId",
                table: "CustomerSuppliers",
                column: "SupplierOrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSuppliers_Organizations_SupplierOrganizationId",
                table: "CustomerSuppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSuppliers_Organizations_SupplierOrganizationId",
                table: "CustomerSuppliers",
                column: "SupplierOrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
