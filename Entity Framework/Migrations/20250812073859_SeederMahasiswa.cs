using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class SeederMahasiswa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mahasiswa",
                columns: new[] { "MahasiswaID", "NIM", "NamaMahasiswa", "ProdiID", "TanggalLahir" },
                values: new object[,]
                {
                    { 1, "123456789", "Budi", 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "987654321", "Siti", 1, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "1122334455", "Andi", 2, new DateTime(2002, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "5566778899", "Dewi", 2, new DateTime(2003, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mahasiswa",
                keyColumn: "MahasiswaID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mahasiswa",
                keyColumn: "MahasiswaID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mahasiswa",
                keyColumn: "MahasiswaID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Mahasiswa",
                keyColumn: "MahasiswaID",
                keyValue: 4);
        }
    }
}
