using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class removedorganizationrelationorganizationtargettable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationTargets_Organizations_OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationTargets_OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTargets_OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationTargets_Organizations_OrganizationId",
                schema: "Reference",
                table: "OrganizationTargets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
