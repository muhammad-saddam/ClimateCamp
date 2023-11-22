using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addeditioninorganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditionId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsContactSales",
                table: "AbpEditions",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EditionId",
                table: "Organizations",
                column: "EditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AbpEditions_EditionId",
                table: "Organizations",
                column: "EditionId",
                principalTable: "AbpEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AbpEditions_EditionId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_EditionId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EditionId",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "IsContactSales",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
