using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    /// <inheritdoc />
    public partial class addedCustomerProductstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProducts_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProducts_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProducts_OrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Reference",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "Reference",
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CreatorUserId",
                schema: "Master",
                table: "CustomerProducts",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_DeleterUserId",
                schema: "Master",
                table: "CustomerProducts",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_LastModifierUserId",
                schema: "Master",
                table: "CustomerProducts",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_OrganizationUnitId",
                schema: "Master",
                table: "CustomerProducts",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_ProductId",
                schema: "Master",
                table: "CustomerProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_UnitId",
                schema: "Master",
                table: "CustomerProducts",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProducts",
                schema: "Master");
        }
    }
}
