using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class rename_my_data_by_activity_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE dbo.AbpFeatures SET Name = 'Activity Data' WHERE Name = 'My Data'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
