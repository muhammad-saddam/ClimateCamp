using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class removeunitfromCustomerProductstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_Units_UnitId",
                schema: "Master",
                table: "CustomerProducts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProducts_UnitId",
                schema: "Master",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "Master",
                table: "CustomerProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "Master",
                table: "CustomerProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_UnitId",
                schema: "Master",
                table: "CustomerProducts",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_Units_UnitId",
                schema: "Master",
                table: "CustomerProducts",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
