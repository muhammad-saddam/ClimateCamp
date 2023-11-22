using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedendoflifetreatmenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndOfLifeTreatmentData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasteTreatmentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndOfLifeTreatmentData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndOfLifeTreatmentData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndOfLifeTreatmentData",
                schema: "Transactions");
        }
    }
}
