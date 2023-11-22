using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class supplierinvitationdatamodelinitialdraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSuppliers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearlyProductQuantity = table.Column<long>(type: "bigint", nullable: true),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearlyServiceQuantity = table.Column<long>(type: "bigint", nullable: true),
                    CO2E = table.Column<float>(type: "real", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSuppliers_Organizations_CustomerOrganizationId",
                        column: x => x.CustomerOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CustomerSuppliers_Organizations_SupplierOrganizationId",
                        column: x => x.SupplierOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSuppliers_CustomerOrganizationId",
                table: "CustomerSuppliers",
                column: "CustomerOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSuppliers_SupplierOrganizationId",
                table: "CustomerSuppliers",
                column: "SupplierOrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSuppliers");
        }
    }
}
