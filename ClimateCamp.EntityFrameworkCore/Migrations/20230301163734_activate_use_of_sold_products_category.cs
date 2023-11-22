﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimateCamp.Migrations
{
    public partial class activate_use_of_sold_products_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE Reference.EmissionsSources SET IsActive = '1' WHERE Name = 'Use of sold products'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
