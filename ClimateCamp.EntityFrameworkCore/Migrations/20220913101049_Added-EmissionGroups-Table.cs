using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class AddedEmissionGroupsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmissionGroups",
                schema: "Reference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmissionSourceId = table.Column<int>(type: "int", nullable: true),
                    ParentEmissionGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmissionGroups_EmissionGroups_ParentEmissionGroupId",
                        column: x => x.ParentEmissionGroupId,
                        principalSchema: "Reference",
                        principalTable: "EmissionGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmissionGroups_EmissionsSources_EmissionSourceId",
                        column: x => x.EmissionSourceId,
                        principalSchema: "Reference",
                        principalTable: "EmissionsSources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmissionGroups_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmissionGroups_EmissionSourceId",
                schema: "Reference",
                table: "EmissionGroups",
                column: "EmissionSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionGroups_OrganizationId",
                schema: "Reference",
                table: "EmissionGroups",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionGroups_ParentEmissionGroupId",
                schema: "Reference",
                table: "EmissionGroups",
                column: "ParentEmissionGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmissionGroups",
                schema: "Reference");
        }
    }
}
