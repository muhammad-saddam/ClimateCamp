using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class added_custom_feature_properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "AbpFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpFeatures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "AbpFeatures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowActiveLabel",
                table: "AbpFeatures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AbpFeatures",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "Evidence",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "SourceTransactionId",
                schema: "Transactions",
                table: "ActivityData");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "AbpFeatures");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpFeatures");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AbpFeatures");

            migrationBuilder.DropColumn(
                name: "ShowActiveLabel",
                table: "AbpFeatures");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AbpFeatures");
        }
    }
}
