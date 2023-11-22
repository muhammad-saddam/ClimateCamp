using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedfugitiveemissionsdatatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FugitiveEmissionsData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GreenhouseGasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FugitiveEmissionsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FugitiveEmissionsData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FugitiveEmissionsData_GreenhouseGases_GreenhouseGasId",
                        column: x => x.GreenhouseGasId,
                        principalSchema: "Reference",
                        principalTable: "GreenhouseGases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FugitiveEmissionsData_GreenhouseGasId",
                schema: "Transactions",
                table: "FugitiveEmissionsData",
                column: "GreenhouseGasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FugitiveEmissionsData",
                schema: "Transactions");
        }
    }
}
