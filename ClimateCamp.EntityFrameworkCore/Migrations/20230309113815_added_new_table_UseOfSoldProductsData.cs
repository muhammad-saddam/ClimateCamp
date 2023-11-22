using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class added_new_table_UseOfSoldProductsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "UseOfSoldProductsData",
            schema: "Transactions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FuelUsedPerUseOfProduct = table.Column<float>(nullable: true),
                FuelUnitId = table.Column<int>(nullable: true),
                ElectricityConsumptionPerUseOfProduct = table.Column<float>(nullable: true),
                ElectricityUnitId = table.Column<int>(nullable: true),
                RefrigerantLeakagePerUseOfProduct = table.Column<float>(nullable: true),
                RefrigerantLeakageUnitId = table.Column<int>(nullable: true)

            },
           constraints: table =>
           {
               table.PrimaryKey("PK_UseOfSoldProducts", x => x.Id);
               table.ForeignKey(
                   name: "FK_UseOfSoldProducts_ActivityData_Id",
                   column: x => x.Id,
                   principalSchema: "Transactions",
                   principalTable: "ActivityData",
                   principalColumn: "Id",
                   onDelete: ReferentialAction.Cascade);
           });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UseOfSoldProductsData",
                schema: "Transactions");
        }
    }
}
