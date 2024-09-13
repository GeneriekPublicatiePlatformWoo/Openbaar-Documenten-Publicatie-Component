using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ODPC.Migrations
{
    /// <inheritdoc />
    public partial class AddGroepenEnWaardelijsten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("4fa89832-2627-412e-b749-4b4da3ee0f93"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("535309b4-d6ef-42eb-8306-9ab834b795d8"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("66a7c17a-2822-4dd1-b9cd-b18d6ebc4bb9"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("cc957f44-67fb-43ee-9eb9-8a263fd80375"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("e31771d6-c6c4-4ff8-a2da-72b90847ac27"));

            migrationBuilder.CreateTable(
                name: "Gebruikersgroepen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruikersgroepen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GebruikersgroepWaardelijsten",
                columns: table => new
                {
                    GebruikersgroepId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaardelijstId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikersgroepWaardelijsten", x => new { x.GebruikersgroepId, x.WaardelijstId });
                    table.ForeignKey(
                        name: "FK_GebruikersgroepWaardelijsten_Gebruikersgroepen_Gebruikersgr~",
                        column: x => x.GebruikersgroepId,
                        principalTable: "Gebruikersgroepen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Gebruikersgroepen",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0e7a0023-423a-421a-8700-359232fef584"), "Groep 3" },
                    { new Guid("8f939b51-dad3-436d-a5fa-495b42317d64"), "Groep 2" },
                    { new Guid("d3da5277-ea07-4921-97b8-e9a181390c76"), "Groep 1" }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GebruikersgroepWaardelijsten");

            migrationBuilder.DropTable(
                name: "Gebruikersgroepen");

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("456386b3-acc6-4883-847c-bee133a508f5"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("9aea5478-2a85-4ebd-9abf-fb0d650b0ec8"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("ab8d9c19-f14d-464a-937f-80ecedfa3ed4"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("b868204b-a2b7-40b7-a73f-a45da611c7d0"));

            migrationBuilder.DeleteData(
                table: "WeatherForecasts",
                keyColumn: "Guid",
                keyValue: new Guid("c6097727-f925-4b74-a720-da47e693a8f0"));

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Guid", "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { new Guid("4fa89832-2627-412e-b749-4b4da3ee0f93"), new DateOnly(2024, 8, 31), "Warm", 25 },
                    { new Guid("535309b4-d6ef-42eb-8306-9ab834b795d8"), new DateOnly(2024, 9, 3), "Chilly", 10 },
                    { new Guid("66a7c17a-2822-4dd1-b9cd-b18d6ebc4bb9"), new DateOnly(2024, 9, 1), "Cool", 15 },
                    { new Guid("cc957f44-67fb-43ee-9eb9-8a263fd80375"), new DateOnly(2024, 9, 2), "Hot", 30 },
                    { new Guid("e31771d6-c6c4-4ff8-a2da-72b90847ac27"), new DateOnly(2024, 8, 30), "Mild", 20 }
                });
        }
    }
}
