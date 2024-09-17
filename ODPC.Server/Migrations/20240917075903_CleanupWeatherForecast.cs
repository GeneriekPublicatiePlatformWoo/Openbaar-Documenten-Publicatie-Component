using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ODPC.Migrations
{
    /// <inheritdoc />
    public partial class CleanupWeatherForecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Guid", "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { new Guid("456386b3-acc6-4883-847c-bee133a508f5"), new DateOnly(2024, 9, 16), "Hot", 30 },
                    { new Guid("9aea5478-2a85-4ebd-9abf-fb0d650b0ec8"), new DateOnly(2024, 9, 17), "Chilly", 10 },
                    { new Guid("ab8d9c19-f14d-464a-937f-80ecedfa3ed4"), new DateOnly(2024, 9, 13), "Mild", 20 },
                    { new Guid("b868204b-a2b7-40b7-a73f-a45da611c7d0"), new DateOnly(2024, 9, 15), "Cool", 15 },
                    { new Guid("c6097727-f925-4b74-a720-da47e693a8f0"), new DateOnly(2024, 9, 14), "Warm", 25 }
                });
        }
    }
}
