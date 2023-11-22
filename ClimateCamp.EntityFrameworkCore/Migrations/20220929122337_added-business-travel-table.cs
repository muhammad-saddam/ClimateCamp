using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedbusinesstraveltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessTravelData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTravelData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessTravelData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessTravelData_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Reference",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTravelData_VehicleTypeId",
                schema: "Transactions",
                table: "BusinessTravelData",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessTravelData",
                schema: "Transactions");
        }
    }
}
