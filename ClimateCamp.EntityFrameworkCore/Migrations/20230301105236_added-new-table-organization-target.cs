using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addednewtableorganizationtarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "OrganizationTargets",
                schema: "Reference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseLineYear = table.Column<int>(type: "int", nullable: false),
                    TargetYear = table.Column<int>(type: "int", nullable: true),
                    TSF = table.Column<int>(type: "int", nullable: true),
                    SBTI = table.Column<int>(type: "int", nullable: true),
                    Scope1Target = table.Column<float>(type: "real", nullable: true),
                    Scope2Target = table.Column<float>(type: "real", nullable: true),
                    Scope3Target = table.Column<float>(type: "real", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationTargets_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTargets_OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "OrganizationTargets",
                schema: "Reference");
        }
    }
}
