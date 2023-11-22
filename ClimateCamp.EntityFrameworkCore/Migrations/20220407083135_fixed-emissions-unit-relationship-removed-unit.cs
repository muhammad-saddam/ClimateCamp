using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ClimateCamp.Migrations
{
    public partial class fixedemissionsunitrelationshipremovedunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emissions_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropIndex(
                name: "IX_Emissions_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.DropColumn(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "Emissions");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                schema: "Reference",
                table: "EmissionsFactorsLibraries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalculationMethod",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalculationOrigin",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                schema: "Reference",
                table: "EmissionsFactors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractualInstruments",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    EnergySource = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractualInstruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LifeCycleActivity",
                schema: "Reference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCycleActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileCombustionData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    DistanceUnitId = table.Column<int>(type: "int", nullable: false),
                    IndustrialProcessType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileCombustionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileCombustionData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileCombustionData_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalSchema: "Reference",
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileCombustionData_Units_DistanceUnitId",
                        column: x => x.DistanceUnitId,
                        principalSchema: "Reference",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MobileCombustionData_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Reference",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                schema: "Reference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StationaryCombustionData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IndustrialProcessType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationaryCombustionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationaryCombustionData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StationaryCombustionData_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalSchema: "Reference",
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedEnergyData",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnergyType = table.Column<int>(type: "int", nullable: false),
                    IsRenewable = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractualInstrumentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedEnergyData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasedEnergyData_ActivityData_Id",
                        column: x => x.Id,
                        principalSchema: "Transactions",
                        principalTable: "ActivityData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedEnergyData_ContractualInstruments_ContractualInstrumentId",
                        column: x => x.ContractualInstrumentId,
                        principalSchema: "Transactions",
                        principalTable: "ContractualInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "LifeCycleActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionsFactors_SectorId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileCombustionData_DistanceUnitId",
                schema: "Transactions",
                table: "MobileCombustionData",
                column: "DistanceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileCombustionData_FuelTypeId",
                schema: "Transactions",
                table: "MobileCombustionData",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileCombustionData_VehicleTypeId",
                schema: "Transactions",
                table: "MobileCombustionData",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedEnergyData_ContractualInstrumentId",
                schema: "Transactions",
                table: "PurchasedEnergyData",
                column: "ContractualInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_StationaryCombustionData_FuelTypeId",
                schema: "Transactions",
                table: "StationaryCombustionData",
                column: "FuelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_LifeCycleActivity_LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "LifeCycleActivityId",
                principalSchema: "Reference",
                principalTable: "LifeCycleActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmissionsFactors_Sector_SectorId",
                schema: "Reference",
                table: "EmissionsFactors",
                column: "SectorId",
                principalSchema: "Reference",
                principalTable: "Sector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_LifeCycleActivity_LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_EmissionsFactors_Sector_SectorId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropTable(
                name: "LifeCycleActivity",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "MobileCombustionData",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "PurchasedEnergyData",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Sector",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "StationaryCombustionData",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "ContractualInstruments",
                schema: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropIndex(
                name: "IX_EmissionsFactors_SectorId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "Reference",
                table: "EmissionsFactorsLibraries");

            migrationBuilder.DropColumn(
                name: "CalculationMethod",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "CalculationOrigin",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "LifeCycleActivityId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.DropColumn(
                name: "SectorId",
                schema: "Reference",
                table: "EmissionsFactors");

            migrationBuilder.AddColumn<int>(
                name: "ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emissions_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emissions_ActivityTypes_ActivityTypeId",
                schema: "Transactions",
                table: "Emissions",
                column: "ActivityTypeId",
                principalSchema: "Reference",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
