using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODPC.Migrations
{
    /// <inheritdoc />
    public partial class GroepenDynamisch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GebruikersgroepId",
                table: "GebruikersgroepWaardelijsten",
                newName: "GebruikersgroepUuid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Gebruikersgroepen",
                newName: "Naam");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Gebruikersgroepen",
                newName: "Uuid");

            migrationBuilder.AddColumn<string>(
                name: "Omschrijving",
                table: "Gebruikersgroepen",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Omschrijving",
                table: "Gebruikersgroepen");

            migrationBuilder.RenameColumn(
                name: "GebruikersgroepUuid",
                table: "GebruikersgroepWaardelijsten",
                newName: "GebruikersgroepId");

            migrationBuilder.RenameColumn(
                name: "Naam",
                table: "Gebruikersgroepen",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Uuid",
                table: "Gebruikersgroepen",
                newName: "Id");
        }
    }
}
