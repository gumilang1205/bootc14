using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fakultas",
                columns: table => new
                {
                    FakultasID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaFakultas = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakultas", x => x.FakultasID);
                });

            migrationBuilder.CreateTable(
                name: "Prodi",
                columns: table => new
                {
                    ProdiID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaProdi = table.Column<string>(type: "TEXT", nullable: false),
                    FakultasID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodi", x => x.ProdiID);
                    table.ForeignKey(
                        name: "FK_Prodi_Fakultas_FakultasID",
                        column: x => x.FakultasID,
                        principalTable: "Fakultas",
                        principalColumn: "FakultasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mahasiswa",
                columns: table => new
                {
                    MahasiswaID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaMahasiswa = table.Column<string>(type: "TEXT", nullable: false),
                    NIM = table.Column<string>(type: "TEXT", nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProdiID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswa", x => x.MahasiswaID);
                    table.ForeignKey(
                        name: "FK_Mahasiswa_Prodi_ProdiID",
                        column: x => x.ProdiID,
                        principalTable: "Prodi",
                        principalColumn: "ProdiID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswa_ProdiID",
                table: "Mahasiswa",
                column: "ProdiID");

            migrationBuilder.CreateIndex(
                name: "IX_Prodi_FakultasID",
                table: "Prodi",
                column: "FakultasID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahasiswa");

            migrationBuilder.DropTable(
                name: "Prodi");

            migrationBuilder.DropTable(
                name: "Fakultas");
        }
    }
}
