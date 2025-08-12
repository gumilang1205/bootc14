using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class SeederMahasiswaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mahasiswa",
                columns: new[] { "MahasiswaID", "NIM", "NamaMahasiswa", "ProdiID", "TanggalLahir" },
                values: new object[] { 5, "123123123", "Driono", 1, new DateTime(2004, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mahasiswa",
                keyColumn: "MahasiswaID",
                keyValue: 5);
        }
    }
}
