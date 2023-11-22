using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class updatedsupplierdatamodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CustomerSuppliers",
                newName: "CustomerSuppliers",
                newSchema: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmailAddress",
                schema: "Transactions",
                table: "CustomerSuppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                schema: "Transactions",
                table: "CustomerSuppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailAddress",
                schema: "Transactions",
                table: "CustomerSuppliers");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                schema: "Transactions",
                table: "CustomerSuppliers");

            migrationBuilder.RenameTable(
                name: "CustomerSuppliers",
                schema: "Transactions",
                newName: "CustomerSuppliers");
        }
    }
}
