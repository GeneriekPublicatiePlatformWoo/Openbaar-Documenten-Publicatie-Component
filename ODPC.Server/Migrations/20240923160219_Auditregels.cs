using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODPC.Migrations
{
    /// <inheritdoc />
    public partial class Auditregels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditregels",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Aanmaakdatum = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Actie = table.Column<int>(type: "integer", nullable: false),
                    ActieWeergave = table.Column<string>(type: "text", nullable: false),
                    GebruikersId = table.Column<string>(type: "text", nullable: false),
                    Resource = table.Column<string>(type: "text", nullable: false),
                    ResourceWeergave = table.Column<string>(type: "text", nullable: true),
                    ResourceUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Wijzigingen_Oud = table.Column<byte[]>(type: "bytea", nullable: true),
                    Wijzigingen_Nieuw = table.Column<byte[]>(type: "bytea", nullable: true),
                    ResultaatWeergave = table.Column<string>(type: "text", nullable: false),
                    Resultaat = table.Column<int>(type: "integer", nullable: false),
                    Geslaagd = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditregels", x => x.Uuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditregels_GebruikersId",
                table: "Auditregels",
                column: "GebruikersId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditregels_Geslaagd",
                table: "Auditregels",
                column: "Geslaagd");

            migrationBuilder.CreateIndex(
                name: "IX_Auditregels_Resource",
                table: "Auditregels",
                column: "Resource");

            migrationBuilder.CreateIndex(
                name: "IX_Auditregels_ResourceUuid",
                table: "Auditregels",
                column: "ResourceUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditregels");
        }
    }
}
