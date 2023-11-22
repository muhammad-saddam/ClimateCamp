using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedemployeecommutetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeCommuteData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCommuteData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCommuteData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeCommuteData_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Reference",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCommuteData_VehicleTypeId",
                schema: "Transactions",
                table: "EmployeeCommuteData",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCommuteData",
                schema: "Transactions");
        }
    }
}
