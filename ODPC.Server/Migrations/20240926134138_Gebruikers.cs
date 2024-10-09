using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODPC.Migrations
{
    /// <inheritdoc />
    public partial class Gebruikers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GebruikersgroepGebruikers",
                columns: table => new
                {
                    GebruikersgroepUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    GebruikerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikersgroepGebruikers", x => new { x.GebruikerId, x.GebruikersgroepUuid });
                    table.ForeignKey(
                        name: "FK_GebruikersgroepGebruikers_Gebruikersgroepen_Gebruikersgroep~",
                        column: x => x.GebruikersgroepUuid,
                        principalTable: "Gebruikersgroepen",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GebruikersgroepGebruikers_GebruikersgroepUuid",
                table: "GebruikersgroepGebruikers",
                column: "GebruikersgroepUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GebruikersgroepGebruikers");
        }
    }
}
