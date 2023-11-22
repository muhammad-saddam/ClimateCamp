using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class addedproductionQuantityandunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HubSpotId",
                table: "Organizations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductionQuantity",
                table: "Organizations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionQuantityUnitId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationIndustries",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationIndustries", x => new { x.OrganizationId, x.IndustryId });
                    table.ForeignKey(
                        name: "FK_OrganizationIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationIndustries_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ProductionQuantityUnitId",
                table: "Organizations",
                column: "ProductionQuantityUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationIndustries_IndustryId",
                table: "OrganizationIndustries",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Units_ProductionQuantityUnitId",
                table: "Organizations",
                column: "ProductionQuantityUnitId",
                principalSchema: "Reference",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Units_ProductionQuantityUnitId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "OrganizationIndustries");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ProductionQuantityUnitId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "HubSpotId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ProductionQuantity",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ProductionQuantityUnitId",
                table: "Organizations");
        }
    }
}
