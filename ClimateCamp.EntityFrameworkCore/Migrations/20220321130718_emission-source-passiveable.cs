using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class emissionsourcepassiveable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsSources_Units_UnitId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsSources_UnitId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Reference",
                table: "EmissionsSources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConnectorId",
                schema: "Transactions",
                table: "ActivityData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Evidence",
                schema: "Transactions",
                table: "ActivityData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceTransactionId",
                schema: "Transactions",
                table: "ActivityData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Reference",
                table: "EmissionsSources");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "Evidence",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "SourceTransactionId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsSources_UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsSources_Units_UnitId",
                schema: "Reference",
                table: "EmissionsSources",
                column: "UnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
