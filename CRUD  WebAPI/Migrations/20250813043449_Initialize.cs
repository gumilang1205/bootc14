using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD__WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FakultasID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaFakultas = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FakultasID);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    ProdiID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaProdi = table.Column<string>(type: "TEXT", nullable: true),
                    FakultasID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.ProdiID);
                    table.ForeignKey(
                        name: "FK_Subjects_Faculties_FakultasID",
                        column: x => x.FakultasID,
                        principalTable: "Faculties",
                        principalColumn: "FakultasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    MahasiswaID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NIM = table.Column<int>(type: "INTEGER", nullable: false),
                    NamaMahasiswa = table.Column<string>(type: "TEXT", nullable: true),
                    ProdiID = table.Column<int>(type: "INTEGER", nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Alamat = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.MahasiswaID);
                    table.ForeignKey(
                        name: "FK_Students_Subjects_ProdiID",
                        column: x => x.ProdiID,
                        principalTable: "Subjects",
                        principalColumn: "ProdiID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProdiID",
                table: "Students",
                column: "ProdiID");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_FakultasID",
                table: "Subjects",
                column: "FakultasID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
