using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class changedorganizationunitidtoorganizationidcustomerproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_OrganizationUnits_OrganizationUnitId",
                schema: "Master",
                table: "CustomerProducts");

            migrationBuilder.RenameColumn(
                name: "OrganizationUnitId",
                schema: "Master",
                table: "CustomerProducts",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerProducts_OrganizationUnitId",
                schema: "Master",
                table: "CustomerProducts",
                newName: "IX_CustomerProducts_OrganizationId");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Master",
                table: "CustomerProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_Organizations_OrganizationId",
                schema: "Master",
                table: "CustomerProducts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_Organizations_OrganizationId",
                schema: "Master",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Master",
                table: "CustomerProducts");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "Master",
                table: "CustomerProducts",
                newName: "OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerProducts_OrganizationId",
                schema: "Master",
                table: "CustomerProducts",
                newName: "IX_CustomerProducts_OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_OrganizationUnits_OrganizationUnitId",
                schema: "Master",
                table: "CustomerProducts",
                column: "OrganizationUnitId",
                principalTable: "OrganizationUnits",
                principalColumn: "Id");
        }
    }
}
