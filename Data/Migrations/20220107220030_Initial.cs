using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BF.WorldometerScraper.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryDailyCases",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Region = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveCases = table.Column<string>(type: "TEXT", nullable: true),
                    TotalCases = table.Column<string>(type: "TEXT", nullable: true),
                    TotalTests = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryDailyCases", x => new { x.CountryName, x.Time });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryDailyCases");
        }
    }
}
