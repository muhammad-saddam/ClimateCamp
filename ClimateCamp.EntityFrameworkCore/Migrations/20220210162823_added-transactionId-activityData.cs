using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedtransactionIdactivityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                schema: "Transactions",
                table: "ActivityData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                schema: "Transactions",
                table: "ActivityData");
        }
    }
}
