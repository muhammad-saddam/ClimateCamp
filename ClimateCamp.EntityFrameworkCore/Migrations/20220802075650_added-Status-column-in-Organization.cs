﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class addedStatuscolumninOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organizations");
        }
    }
}
