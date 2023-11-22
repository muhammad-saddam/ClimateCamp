using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedpropertiesforpathfinder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerAssignedSupplierId",
                schema: "Transactions",
                table: "PurchasedProductsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpcCode",
                schema: "Reference",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditComments",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditCoverage",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuditDate",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditLevel",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditStandardName",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuditUpdateDate",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductFootprintComment",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecVersion",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusComment",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidityPeriodEnd",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidityPeriodStart",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "Transactions",
                table: "ProductEmissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarbonFootprints",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitaryProductAmount = table.Column<double>(type: "float", nullable: true),
                    PcfExcludingBiogenic = table.Column<double>(type: "float", nullable: true),
                    PcfIncludingBiogenic = table.Column<double>(type: "float", nullable: true),
                    FossilGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    FossilCarbonContent = table.Column<double>(type: "float", nullable: true),
                    BiogenicCarbonContent = table.Column<double>(type: "float", nullable: true),
                    DLucGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    LandManagementGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    OtherBiogenicGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    ILucGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    BiogenicCarbonWithdrawal = table.Column<double>(type: "float", nullable: true),
                    AircraftGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    CharacterizationFactors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrossSectoralStandardsUsed = table.Column<int>(type: "int", nullable: true),
                    ProductOrSectorSpecificRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiogenicAccountingMethodology = table.Column<int>(type: "int", nullable: true),
                    BoundaryProcessesDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferencePeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferencePeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeographyCountrySubdivision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeographyCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeographyRegionOrSubregion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryEmissionFactorSources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExemptedEmissionsPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptedEmissionsDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingEmissionsIncluded = table.Column<bool>(type: "bit", nullable: true),
                    PackagingGhgEmissions = table.Column<double>(type: "float", nullable: true),
                    AllocationRulesDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UncertaintyAssessmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DqiCoveragePercent = table.Column<float>(type: "real", nullable: true),
                    TechnologicalDQR = table.Column<double>(type: "float", nullable: true),
                    TemporalDQR = table.Column<double>(type: "float", nullable: true),
                    GeographicalDQR = table.Column<double>(type: "float", nullable: true),
                    CompletenessDQR = table.Column<double>(type: "float", nullable: true),
                    ReliabilityDQR = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarbonFootprints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmissions_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CarbonFootprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEmissions_CarbonFootprints_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions",
                column: "CarbonFootprintId",
                principalSchema: "Transactions",
                principalTable: "CarbonFootprints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEmissions_CarbonFootprints_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropTable(
                name: "CarbonFootprints",
                schema: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEmissions_CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "BuyerAssignedSupplierId",
                schema: "Transactions",
                table: "PurchasedProductsData");

            migrationBuilder.DropColumn(
                name: "CpcCode",
                schema: "Reference",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuditComments",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "AuditCoverage",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "AuditDate",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "AuditLevel",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "AuditStandardName",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "AuditUpdateDate",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "CarbonFootprintId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "ProductFootprintComment",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "SpecVersion",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "StatusComment",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "ValidityPeriodEnd",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "ValidityPeriodStart",
                schema: "Transactions",
                table: "ProductEmissions");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Transactions",
                table: "ProductEmissions");
        }
    }
}
