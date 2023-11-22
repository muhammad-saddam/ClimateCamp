using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateCamp.Migrations
{
    public partial class addedIsFirstLoginExperienceuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLoginExperience",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstLoginExperience",
                table: "AbpUsers");
        }
    }
}
