using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fietsenwinkel.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FietsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FietsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filialen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filialen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voorraden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FiliaalId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voorraden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voorraden_Filialen_FiliaalId",
                        column: x => x.FiliaalId,
                        principalTable: "Filialen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fietsen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FrameMaat = table.Column<int>(type: "INTEGER", nullable: false),
                    FietsTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    VoorraadModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fietsen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fietsen_FietsTypes_FietsTypeId",
                        column: x => x.FietsTypeId,
                        principalTable: "FietsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fietsen_Voorraden_VoorraadModelId",
                        column: x => x.VoorraadModelId,
                        principalTable: "Voorraden",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fietsen_FietsTypeId",
                table: "Fietsen",
                column: "FietsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fietsen_VoorraadModelId",
                table: "Fietsen",
                column: "VoorraadModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Voorraden_FiliaalId",
                table: "Voorraden",
                column: "FiliaalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fietsen");

            migrationBuilder.DropTable(
                name: "FietsTypes");

            migrationBuilder.DropTable(
                name: "Voorraden");

            migrationBuilder.DropTable(
                name: "Filialen");
        }
    }
}
