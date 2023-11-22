using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedtransportanddistributiondataentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportAndDistributionData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportMode = table.Column<int>(type: "int", nullable: true),
                    SupplierOrganization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    GoodsQuantity = table.Column<float>(type: "real", nullable: true),
                    GoodsUnitId = table.Column<int>(type: "int", nullable: true),
                    Distance = table.Column<float>(type: "real", nullable: true),
                    DistanceUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportAndDistributionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportAndDistributionData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAndDistributionData_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Reference",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportAndDistributionData_VehicleTypeId",
                schema: "Transactions",
                table: "TransportAndDistributionData",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportAndDistributionData",
                schema: "Transactions");
        }
    }
}
